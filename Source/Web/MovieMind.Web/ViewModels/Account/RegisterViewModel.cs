namespace MovieMind.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(13,130, ErrorMessage ="You need to be older than {0} to register.")]
        public int Age { get; set; }

        [Required]
        public Genders Gender { get; set; }

        [Required]
        public UserCountries Country { get; set; }
    }
}
