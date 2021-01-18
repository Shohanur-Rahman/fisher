using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Company.Models.CompanyAdmin
{

    [Table("CompanyAdmin")]
    public  class CompanyAdmin
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CompanyID { get; set; }

   
        [StringLength(250)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(500)]
        public string CompanyLogoPath { get; set; }

        [StringLength(250)]
        public string CompanyLogoCaption { get; set; }


        [StringLength(250)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [StringLength(250)]
        [Display(Name = "Phone 1")]
        public string Phone1 { get; set; }


        [StringLength(250)]
        [Display(Name = "Phone 2")]
        public string Phone2 { get; set; }

        [StringLength(250)]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }


        [StringLength(250)]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }


        [StringLength(250)]
        [Display(Name = "Web Address")]
        public string WebAddress { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])[\\w~@#$%^&*+=`|{}:;!.?\"()\\[\\]-]{6,}$", ErrorMessage = "Make your password medium or strong")]
 
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; }


        [Display(Name = "License Start Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? LicenseStartDate { get; set; }

        [Display(Name = "License End Date")]
        [Column(TypeName = "DateTime")]
        public DateTime? LicenseEndDate { get; set; }

        [Display(Name = "How Many License")]
        public long HowManyLicense { get; set; }


        [Display(Name = "Town/City")]

        public string CompanyTown { get; set; }

        [Display(Name = "Post code")]

        public string CompanyPostCode { get; set; }


        public bool IsActive { get; set; }

        public bool CustomURL { get; set; }

        public bool DataUploadForHazardAndTrade { get; set; }

        public string CreatedIP { get; set; }
        public string EditedIP { get; set; }

        public long? CreatedBy { get; set; }

        public long? EditedBy { get; set; }

        public string CreatedByUserRole { get; set; }

        public string EditedByUserRole { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }


    }
}
