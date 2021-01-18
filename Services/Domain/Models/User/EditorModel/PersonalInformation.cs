using System.ComponentModel.DataAnnotations;
using Core.ViewService.Models;

namespace Services.Domain.Models.User.EditorModel
{
    public class PersonalInformation : BaseEditorModel
    {
        public long? UserId { get; set; }


        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First name must be a maximum of 20 characters")]
        [StringLength(20)]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "First name is required.")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "Last name must be a maximum of 20 characters")]
        [StringLength(20)]
        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

         
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Email address is required.")]
        public string EmailAddress { get; set; }

    
        [Display(Name = "Job Role")]
        //[Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; }

        public string UserImageCaption { get; set; }

        public string UserImagePath { get; set; }

        public long UserImageSize { get; set; }
        public string UserCreatedBy { get; set; }
        public bool IsImageUploadedByUser { get; set; }
        public bool? IsTrialUser { get; set; }

        [Display(Name = "Redirect URL")]
        //[Required(ErrorMessage = "Redirect url is required.")]
        public string RedirectUrl { get; set; }


        [Display(Name = "Block Reason")]
        public string BlockReason { get; set; }


        [Display(Name = "Created IP")]
        public string CreatedIP { get; set; }
    }
}




