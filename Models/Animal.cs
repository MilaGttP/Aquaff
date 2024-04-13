using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public bool IsSatisfied {  get; set; }
        public bool IsAdult { get; set; }
        public DateOnly Born { get; set; }
        public DateOnly Dead {  get; set; }
        public string Type { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int AquariumId { get; set; }
        public Aquarium Aquarium { get; set; }
    }
}
