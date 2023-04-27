using System.ComponentModel.DataAnnotations;

namespace Wedding.Models
{
    public class RsvpViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0, 20, ErrorMessage = "Please let us know how many people are coming or enter 0 if you cannot make it.")]
        public byte NumberAttending { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}