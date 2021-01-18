using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Accounts.Models.Feed
{

    public class FeedSellingReportTableDataModal
    {
        public FeedSellingReportTableDomain feedSellingReportTableDomain { get; set; }
        public bool isAddOperation { get; set; }
    }

    //[Table("FeedSellingReport")]
    public class FeedSellingReportTableDomain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedSellingReportId { get; set; }

        public string SellingFeedReportNumber { get; set; }

        public string SellingReportName { get; set; }

        public int FeedId { get; set; }
        public int SellingReportFeedCategoryId { get; set; }

        public int SellingFeedAreaId { get; set; }

        public int SellingFeedProjectId { get; set; }

        public int SellingFeedWeightPerSack { get; set; }

        public decimal SellingFeedSackNumber { get; set; }

        public decimal SellingFeedTotalWeight { get; set; }

        public decimal SellingFeedPricePerKg { get; set; }

        public decimal SellingFeedTotalPrice { get; set; }

        public DateTime SellingFeedCalculationDate { get; set; }

        public DateTime? SellingFeedCreateDate { get; set; }

        public int? SellingFeedCreatedId { get; set; }

        public DateTime? SellingFeedEditedDate { get; set; }

        public int? SellignFeedEditedId { get; set; }

        public string SellingFeedSellNote { get; set; }

    }



    public class SPFeedSellingReportsBySearchParam
    {
        public int FeedSellingReportId { get; set; }
        public string SellingFeedReportNumber { get; set; }
        public string SellingReportName { get; set; }
        public string FullName { get; set; }
        public string SellingFeedCalculationDate { get; set; }
        public string SellingFeedWeightPerSack { get; set; }
        public string FeedCategoryName { get; set; }
        public string MainFishImage { get; set; }
        public string SellingFeedSackNumber { get; set; }
        public string SellingFeedTotalWeight { get; set; }
        public string SellingFeedPricePerKg { get; set; }
        public string SellingFeedTotalPrice { get; set; }
        public string SellingFeedCreateDate { get; set; }
        public string FeedNameWithCat { get; set; }


        public int TotalRows { get; set; }
    }

    public class SPTotalFeedSellingReportsBySearchParam
    {
       
        public string SellingFeedTotalWeight { get; set; }
        public string SellingFeedTotalPrice { get; set; }
        public string SellingFeedPricePerKg { get; set; }
        public string SellingFeedSackNumber { get; set; }
    }

    public class SPFeedSellingUserDashboardOverview
    {
        public string SellingFeedPricePerKg { get; set; }
        public string CalculationDate { get; set; }
    }

    public class SPFeedBuyingReportByFeedCategory
    {
        public string SellingFeedTotalPrice { get; set; }
        public string FeedCategory { get; set; }
    }


    public class up_GetFeedPurchaseReportByAreaId {

        public int FeedSellingReportId { get; set; }
        public string SellingFeedReportNumber { get; set; }
        public string SellingReportName { get; set; }
        public string FullName { get; set; }
        public string SellingFeedCalculationDate { get; set; }
        public string SellingFeedTotalWeight { get; set; }
        public string SellingFeedTotalPrice { get; set; }
        public string FeedAmountPaid { get; set; }
        public string FeedAmountDue { get; set; }
        public string SellingFeedCreateDate { get; set; }
        public string FeedNames { get; set; }
        public string ProjectName { get; set; }


    }

}
