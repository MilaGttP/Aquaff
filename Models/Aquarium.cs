using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Aquarium
    {
        [Key]
        public int Id { get; set; }
        public bool IsDirty { get; set; }
        public DateTime Bought {  get; set; }
        //public ICollection<Animal> Animals { get; set; }
    }
}
