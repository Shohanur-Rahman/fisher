using Core.ViewService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Models.User
{
    public class CompanyUserModel : BaseEditorModel
    {
        [Display(Name = "UserID")]
        public long? UserID { get; set; }

        [Display(Name = "UserRegistartionGuid")]
        public string UserRegistartionGuid { get; set; }

        [Display(Name = "নামের প্রথমাংশ")]
        [MaxLength(20, ErrorMessage = "First name must be a maximum of 20 characters")]
        [StringLength(20)]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }


        [Display(Name = "নামের শেষাংশ")]
        [MaxLength(20, ErrorMessage = "Last name must be a maximum of 20 characters")]
        [StringLength(20)]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }


        [Display(Name = "User name")]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "User name is required.")]
        public string UserName { get; set; }

        [Display(Name = "ইমেল")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [RegularExpression(@"^(?=.*\S).+$", ErrorMessage = "Email address is required.")]
        public string EmailAddress { get; set; }

        [Display(Name = "কাজের ভূমিকা")]
        public string Position { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "পাসওয়ার্ড")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])[\\w~@#$%^&*+=`|{}:;!.?\"()\\[\\]-]{6,}$", ErrorMessage = "Make your password medium or strong")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "পাসওয়ার্ড নিশ্চিত করুন")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public string UserImagePath { get; set; }

        public string UserImageCaption { get; set; }       
        
        public int AreaId { get; set; }
        public int ProjectID { get; set; }
        public string IsUserActivated { get; set; }
        public string UserRole { get; set; }
        public string CreatedIP { get; set; }
        public bool IsAddOperation { get; set; }

    }

    public class SPUserSearchListResult {

        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string UserFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserImagePath { get; set; }

        public string ProjectDetails { get; set; }
    }


    public class SPFishSellingChartReport {
        public string TotalSell { get; set; }
        public string ProjectName { get; set; }
    }

    public class SPAdminDashBoardOveriew
    {
        public decimal? TotalFishSell { get; set; }
        public decimal? TotalFeedBuy { get; set; }
        public decimal? TotalFeedDistribute { get; set; }
        public decimal? TotalRecord { get; set; }
    }


}
