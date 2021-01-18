using System.ComponentModel.DataAnnotations;
using Core.ViewService.Models;

namespace Services.Domain.Models.User.EditorModel
{
    public class UserPassword : BaseEditorModel
    {
        public long? UserId { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Current password is required")]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(100)]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])[\\w~@#$%^&*+=`|{}:;!.?\"()\\[\\]-]{6,}$", ErrorMessage = "Make your password medium or strong")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [StringLength(100)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        public string ConfirmPassword { get; set; }

    }
}






