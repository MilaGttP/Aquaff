using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isAdult { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Animal> Animals { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
