using Aquaff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aquaff.Pages
{
    public class AquariumModel : PageModel
    {
        private readonly DBContext _context;

        public AquariumModel(ILogger<IndexModel> logger, DBContext context)
        {
            _context = context;
        }

        public Aquarium Aquarium { get; set; }
        public int NotifyCount { get; set; }
        public DBContext Context { get; set; }

        public async Task OnGetAsync()
        {
            Context = _context;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = await _context.Accounts.FindAsync(userId.Value);
                if (user != null)
                {
                    Aquarium = await _context.Aquariums.FindAsync(user.AquariumId);
                    NotifyCount = _context.Notifications.Where(n => n.AccountId == userId).ToList().Count();
                }
            }

            await MakeHungry();
            await MakeDirty();
            await NotifyAboutBuy();
            await AnimalDeadNotify();
            await MoreAnimals();
        }

        public async Task<IActionResult> OnPostFeedAnimalAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            List<Animal> animals = _context.Animals.Where(a => a.AquariumId == userId).ToList();
            foreach (Animal animal in animals)
            {
                animal.IsSatisfied = true;
            }
            Account user = await _context.Accounts.FindAsync(userId);
            if (user.Money > 40)
            {
                user.Money -= 40;
                Notification notification = new Notification { Header = "Ви погодували мешканця!", Text = "Тепер він буде поки що витримувати це складне життя!", AccountId = userId };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Aquarium");
        }

        public async Task<IActionResult> OnPostCleanAquariumAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Accounts.FindAsync(userId);
            Aquarium = await _context.Aquariums.FindAsync(user.AquariumId);
            if (user.Money > 40)
            {
                user.Money -= 40;
                Aquarium.IsDirty = false;
                Notification notification = new Notification { Header = "Ви помили акваріум!", Text = "Тепер акваріум чистий!", AccountId = userId };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToPage("/Aquarium");
        }

        public async Task MakeHungry()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            DateTime currentTime = DateTime.Now;
            List<Animal> animals = _context.Animals.Where(a => a.AquariumId == userId).ToList();
            if ((currentTime.Hour == 9 && currentTime.Minute == 0 && currentTime.Second == 0)
                || (currentTime.Hour == 15 && currentTime.Minute == 0 && currentTime.Second == 0)
                || (currentTime.Hour == 21 && currentTime.Minute == 0 && currentTime.Second == 0))
            {
                foreach (var animal in animals)
                {
                    animal.IsSatisfied = false;
                }

                Notification notification = new Notification { Header = "Ваші рибки зголодніли!", Text = "Погодуйте їх, а то вони не витримають :(", AccountId = userId };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MakeDirty()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            DateTime currentTime = DateTime.Now;
            Aquarium aquarium = _context.Aquariums.Find(userId);
            if (currentTime.Hour == 6 && currentTime.Minute == 0 && currentTime.Second == 0)
            {
                aquarium.IsDirty = true;

                Notification notification = new Notification { Header = "Ваш акваріум брудний!", Text = "Помийте його!", AccountId = userId };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task NotifyAboutBuy()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            List<Animal> animals = _context.Animals.Where(a => a.AquariumId == userId).ToList();
            if (animals.Count >= 4)
            {
                Notification notification = new Notification { Header = "Ви можете заробити!", Text = "У профілі ви можете продати вашого мешканця та заробити грошей", AccountId = userId };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AnimalDeadNotify()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            List<Animal> animals = _context.Animals.Where(a => a.AquariumId == userId).ToList();
            foreach (var animal in animals)
            {
                if (animal.Dead == DateTime.Now)
                {
                    Notification notification = new Notification { Header = "Ваша рибка не витримала!", Text = "На превеликий жаль, ваша рибка не витримала своєї важкої долі...", AccountId = userId };
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task MoreAnimals()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            DateTime currentTime = DateTime.Now;

            List<Animal> animals = _context.Animals.Where(a => a.AquariumId == userId).ToList();

            var groupedAnimals = animals.GroupBy(a => a.Type);

            foreach (var group in groupedAnimals)
            {
                if (group.Count() >= 2 && currentTime.Hour == 10 && currentTime.Minute == 0 && currentTime.Second == 0)
                {
                    Notification notification = new Notification { Header = "Юхууу!", Text = "Ваших " + group.Key + " стало більше", AccountId = userId };
                    _context.Notifications.Add(notification);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
