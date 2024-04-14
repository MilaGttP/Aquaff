using Aquaff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aquaff.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DBContext _context;

        public RegisterModel(ILogger<IndexModel> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Account NewAccount { get; set; }

        public void OnGet()
        {

        }

        Animal GetAnimal()
        {
            Animal animal = new Animal();
            animal.IsSatisfied = true;
            animal.IsAdult = true;
            animal.Born = DateTime.Now;
            Random rnd = new Random();
            int daysToAdd = rnd.Next(5, 21);
            animal.Dead = DateTime.Now.AddDays(daysToAdd);
            animal.Type = "Fish";
            animal.Price = 50;
            animal.Account = NewAccount;
            return animal;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            NewAccount.Money = 300;
            var aquarium = new Aquarium { IsDirty = false, Bought = DateTime.Now };
            _context.Aquariums.Add(aquarium);

            NewAccount.Aquarium = aquarium;

            _context.Accounts.Add(NewAccount);
            var fish = GetAnimal();
            fish.Aquarium = aquarium;
            var fish1 = GetAnimal();
            fish1.Aquarium = aquarium;

            var notify = new Notification { Header = "Вітаємо на Aquaff!", Text = "Дякуємо за рестрацію! Гарного користування :)", Account = NewAccount};

            _context.Animals.Add(fish);
            _context.Animals.Add(fish1);
            _context.Notifications.Add(notify);

            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", NewAccount.Id);

            return RedirectToPage("/Aquarium");
        }
    }
}
