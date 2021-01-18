using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Accounts.Models.Fish
{
    public class FishSellingReportTableDataModal
    { 
        public FishSellingReportTableDomain FishSellingReportTableDomain { get; set; }
        public IList<FishSellingReportMapperTable> FishSellingMapperList { get; set; }
        public string FishIdListWithComma { get; set; }
        public string ExistingMapperIdListForEdit { get; set; }
        public FishAmountDueReportMapperTable FishAmountDueReportMapperTable { get; set; }
        public bool isAddOperation { get; set; }

    }

    [Table("FishSellingReport")]
    public class FishSellingReportTableDomain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FishSellId { get; set; }
        
        [StringLength(250)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "হিসাবের নাম লিখুন")]
        public string HisabName { get; set; }

        [StringLength(250)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "গাড়ীর নাম লিখুন")]
        public string GarirName { get; set; }

        [Required(ErrorMessage = "মোট মাছ বিক্রি (কেজি) নির্ধারণ করুন")]
        public decimal TotalSellInKG { get; set; }

        [Required(ErrorMessage = "বিক্রির তারিখ নির্ধারণ করুন")]
        public DateTime SellDate { get; set; }

        //[Required(ErrorMessage = "মূলত যেই মাছটি বিক্রি করেছেন তার নাম নিশ্চিত করুন")]
        //public int SellFishId { get; set; }

        public int FishSellerId { get; set; }

        //[Required(ErrorMessage = "প্রতি কেজিতে মাছের সংখ্যা নির্ধারণ করুন")]
        //public int PiecesPerKG { get; set; }

        //[Required(ErrorMessage = "মোট মাছের সংখ্যা নির্ধারণ করুন")]
        //public int TotalPiecesFish { get; set; }

        //[Required(ErrorMessage = "প্রতি কেজির মূল্য বিক্রি মূল্য নির্ধারণ করুন")]
        //public decimal PricePerKG { get; set; }

        [Required(ErrorMessage = "মোট বিক্রি মূল্য নির্ধারণ করুন")]
        public decimal TotalSellPrice { get; set; }

        //public string FishImageUrl { get; set; }

        public DateTime? CalculatedDate { get; set; }

        public int? CalculatedById { get; set; }

        public DateTime? CalculationEditedDate { get; set; }

        public int? CalCulationEditedId { get; set; }

        public string SellNote { get; set; }

        public int? AreaId { get; set; }
        public int? ProjectId { get; set; }

        public decimal FishAmountPaid { get; set; }
        public decimal FishAmountDue { get; set; }
        public int IsClosedByAdmin { get; set; }
    }


    public class SPFishSellingReportListBySearchParam
    {
        public int FishSellId { get; set; }
        public string HisabName { get; set; }
        public string FishSellingDate { get; set; }
        public string TotalSellInKG { get; set; }
        public string SellerName { get; set; }
        public string TotalSellPrice { get; set; }
        public string CalculatedDate { get; set; }
        public int AreaId { get; set; }
        public int ProjectId { get; set; }
        public string AllFishName { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
        public string ProjectName { get; set; }
        public string AreaName { get; set; }
        public string ProjectWithArea { get; set; }
        public int TotalRows { get; set; }
    }



    public class SPFishSellingReportGetTotalRecord
    {
        public string TOTAL_SELL_IN_KG { get; set; }
        public string TotalSellPrice { get; set; }
        public string PricePerKg { get; set; }
        public string TotalPiecesFish { get; set; }
        public string FishAmountDue { get; set; }
        public string FishAmountPaid { get; set; }
        public int TotalRows { get; set; }
    }



    public class SPFishSellingReportListForAdminBySearchParam
    {
        public int FishSellId { get; set; }
        public string SellingFishName { get; set; }
        public string HisabName { get; set; }
        public string FishSellingDate { get; set; }
        public string TotalSellInKG { get; set; } 
        public string TotalSellPrice { get; set; }
        public string CalculatedDate { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
        public string SellerName { get; set; } 
        public int AreaId { get; set; }
        public int ProjectId { get; set; }
        public int TotalRows { get; set; }
    }


    public class SPFishSellingSellerReportForAdminByParam
    {
        public int FishSellId { get; set; }
        public string SDate { get; set; }
        public string Name { get; set; }
        public int projectid { get; set; }
        public string SellDate { get; set; }
        public int FishSellerId { get; set; }
        public string totalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
        public string ProjectName { get; set; }
        public int TotalRows { get; set; }
    }


    public class SPFishSellingUserDashboardOverview
    {
        public string TotalSellPrice { get; set; }
        public string CalculatedDate { get; set; }
    }


    public class SPFishSellingReportGroupByFishName
    {
        public string TotalSellPrice { get; set; }
        public string FishName { get; set; }
    }


    public class SPFishSellingReportGetTotalForAdminRecord
    {
        public string TotalSellInKg { get; set; }
        public string TotalSellPrice { get; set; }
        public string PricePerKg { get; set; }
        public string TotalPiecesFish { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
        public int TotalRows { get; set; }
    }


    public class FishSellPDFInforForUser {
        public FishSellPDFInfoForUser FishInfo { get; set; }
        public IList<SingleFishSellReportList> SingleFishSellReprot { get; set; }

    }

    public class FishSellPDFInfoForUser {
        public string HisabName { get; set; }
        public string GarirName { get; set; }
        public DateTime SellDate { get; set; }
        public int FishSellId { get; set; }
        public string FishSellingDate { get; set; }
        public string TotalSellInKG { get; set; }
        public string TotalSellPrice { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
        public string SellerName { get; set; }
    }

    public class SingleFishSellReportList {

        public string FishName { get; set; }
        public string FishSellerName { get; set; }
        public string TotalFishkg { get; set; }
        public string TotalSellPrice { get; set; }
        public string PiecesPerKG { get; set; }

    }




}
