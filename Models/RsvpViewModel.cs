using System.ComponentModel.DataAnnotations;

namespace Wedding.Models
{
    public class RsvpViewModel{
        [Required]
        public string Name{get;set;}
        [Required]
        [Range(1, 20, ErrorMessage = "You must respond for at least one person.")]
        public int NumberAttending{get;set;}
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber{get;set;}
    }
}