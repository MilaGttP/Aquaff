using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Aquaff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Aquaff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DBContext _context;

        public IndexModel(ILogger<IndexModel> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == Email && a.Password == Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неправильний email або пароль.");
                return Page();
            }

            var claims = new[] { new Claim(ClaimTypes.Name, user.Name) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage("/Aquarium");
        }
    }
}
