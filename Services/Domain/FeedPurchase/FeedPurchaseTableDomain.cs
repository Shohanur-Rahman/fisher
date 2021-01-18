using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.FeedPurchase
{
    public class FeedPurchaseTableDomain
    {
        public FeedPurcaseTable FeedPurcaseTable { get; set; }
        public IList<FeedPurchaseReportTable> FeedPurchaseReportTable { get; set; }
        public string AreaIdListWithComma { get; set; }
        public string ExistingMapperIdListForEdit { get; set; }
        public FeedPurchaseDueReportMapperTable FeedPurchaseDueReportMapperTable { get; set; }
        public bool isAddOperation { get; set; }
    }

    [Table("FeedPurchaseDueReportMapper")]
    public class FeedPurchaseDueReportMapperTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedPurchaseDueReportMapperId { get; set; }
        public int FeedPurchaseId { get; set; }
        public decimal FeedPurchaseAmountDue { get; set; }

    }

    [Table("FeedSellingReport")]
    public class FeedPurcaseTable {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedSellingReportId { get; set; }
        public string SellingFeedReportNumber { get; set; }
        public string SellingReportName { get; set; }
        public decimal SellingFeedTotalWeight { get; set; }
        public decimal SellingFeedTotalPrice { get; set; }
        public DateTime SellingFeedCalculationDate { get; set; }
        public DateTime? SellingFeedCreateDate { get; set; }
        public int? SellingFeedCreatedId { get; set; }
        public DateTime? SellingFeedEditedDate { get; set; }
        public int? SellignFeedEditedId { get; set; }
        public string SellingFeedSellNote { get; set; }
        public int IsClosedByAdmin { get; set; }
        public decimal FeedAmountPaid { get; set; }
        public decimal FeedAmountDue { get; set; }
        public int AreaId { get; set; }
        public int ProjectId { get; set; }

    }



     
    [Table("FeedSellingReportMapper")]
    public class FeedPurchaseReportTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedSellingReportMapperId { get; set; }
        public int FeedSellingReportId { get; set; }
        public int FeedId { get; set; }
        public int FeedCategoryId { get; set; }
        public int FeedTotalBags { get; set; }
        public int FeedBagsWeight { get; set; }     
        public decimal TotalWeight { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal TotalPrice { get; set; }
        public int MapperAreaId { get; set; }
    }


    public class FeedPurchaseSearchResultByParam {
        public IList<FeedPurchaseTableInfoList> FeedPurchaseTableInfoList { get; set; }
        public FeedPurchaseTableTotal FeedPurchaseTableTotal { get; set; }
    }


    public class FeedPurchaseTableTotal {      
        public string SellingFeedTotalWeight { get; set; }
        public string SellingFeedTotalPrice { get; set; }
        public string FeedAmountPaid { get; set; }
        public string FeedAmountDue{ get; set; }

    }
    public class FirstPageFeedPurchaseTableTotal
    {
        public string TotalWeight { get; set; }
        public string TotalPrice { get; set; }

    }

    public class FeedPurchaseTableInfoList {
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


    public class FeedPurchasePDFInfo { 
        public IList<SingleFeedPurchaseReport> feedReportList { get; set; }
        public TotalFeedPurchaseReportForEachId totalReportEachItem { get; set; }
        public FeedPurcaseTable FeedPurcaseTable { get; set; }
    }

    public class TotalFeedPurchaseReportForEachId {
        public string FeedTotalBags { get; set; }
        public string FeedBagsWeight { get; set; }
        public string TotalWeight { get; set; }
        public string TotalPrice { get; set; }
        public string PricePerKg { get; set; }
    }

    public class SingleFeedPurchaseReport {
        public int FeedSellingReportId { get; set; }
        public string AreaName { get; set; }
        public string FeedName { get; set; }
        public string CategoryName { get; set; }
        public string FeedTotalBags { get; set; }
        public string FeedBagsWeight { get; set; }
        public string TotalWeight { get; set; }
        public string TotalPrice { get; set; }
        public string PricePerKg { get; set; }
    }

    public class up_GetFeedPurchasePieChart {
        public string TotalWeight { get; set; }
        public string AreaName { get; set; }
    }



}
