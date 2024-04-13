using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Aquarium
    {
        [Key]
        public int Id { get; set; }
        public bool IsDirty { get; set; }
        public ICollection<Animal> Animals { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
