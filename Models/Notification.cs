using System.ComponentModel.DataAnnotations;

namespace Aquaff.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text {  get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
