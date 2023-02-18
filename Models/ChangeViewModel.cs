using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wedding.Models
{
    public class ChangeViewModel
    {
        public ChangeViewModel() { }
        public ChangeViewModel(Data.Guest guest)
        {
            UserId = guest.UserId;
            NumberAdults = guest.NumberAdults;
            NumberChildren = guest.NumberChildren;
            GuestName = guest.GuestName;
            Email = guest.Email;
            PhoneNumber = guest.PhoneNumber;
            TempId = Guid.NewGuid();
        }

        public Guid? TempId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [Required]
        [DisplayName("Number of Adults")]
        public byte NumberAdults { get; set; }
        [Required]
        [DisplayName("Number of Children")]
        public byte NumberChildren { get; set; }
        [Required]
        [DisplayName("Name")]
        public string? GuestName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

    }
}