using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public bool IsSatisfied {  get; set; }
        public bool IsAdult { get; set; }
        public DateTime Born { get; set; }
        public DateTime Dead {  get; set; }
        public string Type { get; set; }
        public int Price { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int AquariumId { get; set; }
        public Aquarium Aquarium { get; set; }
    }
}
