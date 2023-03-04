using System.ComponentModel.DataAnnotations;

namespace Wedding.Models
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase, 1 lowercase, 1 symbol, and 1 number")]
        public string Password { get; set; } = null!;

    }

    public class SignUpViewModel:SignInViewModel
    {
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }

    
}
