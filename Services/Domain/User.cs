using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Domain
{

    
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserID { get; set; }

        public int ProjectID { get; set; }
        
        public bool? IsTemporaryPassword { get; set; }

        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } 


        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [StringLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        [Display(Name = "Registration Confirmed")]
        public string RegistrationConfirmed { get; set; }
        
        public int? AreaID { get; set; }

        [Display(Name = "User Image Caption")]
        public string UserImageCaption { get; set; }

        [Display(Name = "User Image Path")]
        public string UserImagePath { get; set; }

        public long? UserImageSize { get; set; }

        [StringLength(200)]
        [Display(Name = "Address One")]
        public string AddressOne { get; set; }

        [StringLength(200)]
        [Display(Name = "Address Two")]
        public string AddressTwo { get; set; }

        [StringLength(100)]
        [Display(Name = "Country")]
        public string Country { get; set; }
        
        public string Religion { get; set; }

        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(50)]
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }


        [StringLength(50)]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^(07)[0-9]{9}$", ErrorMessage = "Mobile number must start with 07 and must be 11 digits")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Position")]
        public string Position { get; set; }


        [Display(Name = "Publication Status")]
        [Column(TypeName = "bit")]
        public bool? PublicationStatus { get; set; }

        [Display(Name = "Created By")]
        public long? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Edited By")]
        public long? EditedBy { get; set; }

        [Display(Name = "Edited Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? EditedDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public bool IsActivated { get; set; }

        [Display(Name = "UserRegistartionGuid")]
        public string UserRegistartionGuid { get; set; }

        public bool? IsTrialUser { get; set; }

        [Display(Name = "Trial Start Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? TrialStartDate { get; set; }

        [Display(Name = "Trial End Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? TrialEndDate { get; set; }

        public bool IsImageUploadedByUser { get; set; }

        public string UserCreatedBy { get; set; }

        public string CreatedIP { get; set; }

        public string EditedIP { get; set; }


    }
}
