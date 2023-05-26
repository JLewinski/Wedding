using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Wedding.Data;

namespace Wedding.Models
{
    public class RsvpViewModel : IRsvpModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0, 20, ErrorMessage = "Please let us know how many people are coming or enter 0 if you cannot make it.")]
        public byte NumberAttending { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

    }

    public interface IRsvpModel
    {
        string Name { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
    }

    public static class RsvpExtensions
    {
        public static bool ReplaceFillerInfo(this IRsvpModel viewModel)
        {
            bool changed = false;

            if (viewModel.Email == "jacob@lewinskitech.com")
            {
                viewModel.Email = string.Empty;
                changed = true;
            }

            if (viewModel.PhoneNumber == "2562034011")
            {
                viewModel.PhoneNumber = string.Empty;
                changed = true;
            }

            return changed;
        }
    }
}