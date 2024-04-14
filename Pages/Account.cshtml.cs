using Aquaff.Models;
using Aquaff.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Aquaff.Pages
{
    public class AccountModel : PageModel
    {
        private readonly DBContext _context;

        public AccountModel(DBContext context)
        {
            _context = context;
            Context = context;
        }

        public Account User { get; set; }
        public DBContext Context { get; set; }
        public List<string> AnimalTypes { get; set; }

        public async Task OnGetAsync()
        {
            AnimalTypes = new List<string> { "Fish", "Shrimp", "Snail" };
            Context = _context;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                User = await _context.Accounts.FindAsync(userId.Value);
            }
        }

        public async Task<IActionResult> OnPostSellAnimalAsync(string animalType)
        {
            var profileService = new ProfileService();
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User = await _context.Accounts.FindAsync(userId);

            Animal animal = profileService.GetAnimalByType(_context, userId, animalType);

            if (animal != null)
            {
                var price = profileService.GetPriceByType(_context, userId, animalType);
                User.Money += price;
                Notification notification = new Notification { Header = "Ви продали мешканця!", Text = "Тепер він буде жити в іншому акваріумі, але не сумуйте!", AccountId = userId };

                _context.Animals.Remove(animal);
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Aquarium");
        }

        public async Task<IActionResult> OnPostBuyFishAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User = await _context.Accounts.FindAsync(userId);

            var price = 40;
            if (User.Money > price)
            {
                User.Money -= price;
                Random rnd = new Random();
                int daysToAdd = rnd.Next(5, 21);
                Animal fish = new Animal { IsSatisfied = true, IsAdult = true, Born = DateTime.Now, Dead = DateTime.Now.AddDays(daysToAdd), Type = "Fish", Price = 50, AccountId = userId, AquariumId = userId };
                Notification notification = new Notification { Header = "Ви придбали рибку!", Text = "Тепер вона буде жити у вашому акваріумі! Ура!", AccountId = userId };
                
                _context.Animals.Add(fish);
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Aquarium");
        }

        public async Task<IActionResult> OnPostBuyShrimpAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User = await _context.Accounts.FindAsync(userId);

            var price = 50;
            if (User.Money > price)
            {
                User.Money -= price;
                Random rnd = new Random();
                int daysToAdd = rnd.Next(7, 28);
                Animal shrimp = new Animal { IsSatisfied = true, IsAdult = true, Born = DateTime.Now, Dead = DateTime.Now.AddDays(daysToAdd), Type = "Shrimp", Price = 60, AccountId = userId, AquariumId = userId };
                Notification notification = new Notification { Header = "Ви придбали креветку!", Text = "Тепер вона буде жити у вашому акваріумі! Ура!", AccountId = userId };

                _context.Animals.Add(shrimp);
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Aquarium");
        }

        public async Task<IActionResult> OnPostBuySnailAsync()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            User = await _context.Accounts.FindAsync(userId);

            var price = 30;
            if (User.Money > price)
            {
                User.Money -= price;
                Random rnd = new Random();
                int daysToAdd = rnd.Next(4, 12);
                Animal snail = new Animal { IsSatisfied = true, IsAdult = true, Born = DateTime.Now, Dead = DateTime.Now.AddDays(daysToAdd), Type = "Snail", Price = 40, AccountId = userId, AquariumId = userId };
                Notification notification = new Notification { Header = "Ви придбали равлика!", Text = "Тепер він буде жити у вашому акваріумі! Ура!", AccountId = userId };

                _context.Animals.Add(snail);
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Aquarium");
        }

        public async Task<IActionResult> OnPostDeleteNotificationAsync(int notificationId)
        {
            Notification notification = await _context.Notifications.FindAsync(notificationId);

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account");
        }
    }
}
