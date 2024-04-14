using Aquaff.Models;

namespace Aquaff.Services
{
    public interface IProfileService
    {
        IEnumerable<Models.Notification> GetAllNotificationsByUser(DBContext _context, int userId);
        IEnumerable<Models.Animal> GetAllAnimalsByUser(DBContext _context, int userId);
        int GetCountAnimalByType(DBContext _context, int userId, string type);
        int GetPriceByType(DBContext _context, int userId, string type);
        string GetAnimalPictureByType(string type);
        Animal GetAnimalByType(DBContext _context, int userId, string type);
    }

    public class ProfileService : IProfileService
    {
        public IEnumerable<Animal> GetAllAnimalsByUser(DBContext _context, int userId)
        {
            return _context.Animals.Where(a => a.AccountId == userId).ToList();
        }

        public IEnumerable<Notification> GetAllNotificationsByUser(DBContext _context, int userId)
        {
            return _context.Notifications.Where(n => n.AccountId == userId).ToList();
        }

        public Animal GetAnimalByType(DBContext _context, int userId, string type)
        {
            return GetAllAnimalsByUser(_context, userId).FirstOrDefault(a => a.Type == type);
        }

        public string GetAnimalPictureByType(string type)
        {
            if (type == "Fish")
            {
                return "./images/fish__1.png";
            }
            else if (type == "Shrimp")
            {
                return "./images/shrimp.png";
            }
            else if (type == "Snail")
            {
                return "./images/snail.png";
            }
            else
            {
                return "./images/fish__1.png";
            }
        }

        public int GetCountAnimalByType(DBContext _context, int userId, string type)
        {
            IEnumerable<Animal> animals = GetAllAnimalsByUser(_context, userId);
            return _context.Animals.Where(a => a.Type == type && a.AccountId == userId).Count();
        }

        public int GetPriceByType(DBContext _context, int userId, string type)
        {
            Animal animal = GetAllAnimalsByUser(_context, userId).FirstOrDefault(a => a.Type == type);
            return animal != null ? animal.Price : 0;
        }
    }
}
