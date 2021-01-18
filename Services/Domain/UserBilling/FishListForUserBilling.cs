using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.UserBilling
{
    public class FishSellingDueBillingInfo {
        public IList<FishListForUserBilling> FisnSellingReportList { get; set; }
        public TotalDueBilling TotalBill { get; set; }
    }

    [Table("FishBillingHistory")]
    public class FishBillingHistoryTable {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FishBillingId { get; set; }
        public int FishBillingFishSellId { get; set; }
        public decimal FishBillingFishInKG { get; set; }
        public decimal FishBillingTotalPrice { get; set; }
        public decimal FishBillingPaidAmount { get; set; }
        public decimal FishBillingDueAmount { get; set; }
        public int FishBillingUserId { get; set; }
        public DateTime FishBillingDate { get; set; }
        public string BillingType { get; set; }
    }

    public class TotalDueBilling {
        public string totalAmountString { get; set; }
        public string PaidAmountString { get; set; }
        public string DueAmountString { get; set; }
        public decimal totalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
    }

    public class FishListForUserBilling
    {
        public string Name { get; set; }
        public int FishSellerId { get; set; }
        public int FishSellId { get; set; }
        public string totalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
    }

    public class BillingAmountHistoryBySellerId {
        public IList<ListOfBillingAmountForSeller> BillingList { get; set; }
        public FishSellingDueAndTotalAmount TotalBilling { get; set; } 

    }

    public class ListOfBillingAmountForSeller {
        public IList<FishDueAmountListFishSellMapperList> SellListMapperItems { get; set; }
        public string SellDate { get; set; }
        public int FishSellId { get; set; }
        public string SellingFishName { get; set; }
        public string SellerName { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
        public string TotalSellPrice { get; set; }
        public string TotalSellInKG { get; set; }
    }

    public class FishSellingDueAndTotalAmount {
        public int FishSellId { get; set; }
        public decimal TotalSellInKG { get; set; }
        public decimal FishAmountPaid { get; set; }
        public decimal FishAmountDue { get; set; }
        public decimal TotalSellPrice { get; set; }
        public string TotalSellInKGString { get; set; }
        public string FishAmountPaidString { get; set; }
        public string FishAmountDueString { get; set; }
        public string TotalSellPriceString { get; set; }
    }


    public class FishSellingBillingHistoryList{
        public int FishBillingFishSellId { get; set; }
        public string FishBillingFishInKG { get; set; }
        public string FishBillingTotalPrice { get; set; }
        public string FishBillingPaidAmount { get; set; }
        public string FishBillingDueAmount { get; set; }
        public string SellDate { get; set; }
        public int FishSellerId { get; set; }
        public string SellingFishName { get; set; }
        public string SellerName { get; set; }
        public string FishBillingDate { get; set; }
    }


    public class FishDueAmountListFishSellMapperList {
        public string TotalFishkg { get; set; }
        public string TotalPiecesFish { get; set; }
        public string PricePerKG { get; set; }
        public string TotalSellPrice { get; set; }
        public string FishName { get; set; }
    }

    public class FeedBillingReport {
        public up_GetTotalFeedBillingReportByFeedId TotalBilling { get; set; }
        public IList<up_GetFeedBillingReportByFeedId> BillingList{ get; set; }
    }


    public class up_GetTotalFeedBillingReportByFeedId
    {
        public string TOTALPriceString { get; set; }
        public string FeedTotalBagsString { get; set; }
        public string FeedBagsWeightString { get; set; }
        public string TotalWeightString { get; set; }
        public decimal TOTALPrice { get; set; }
        public int FeedTotalBags { get; set; }
        public int FeedBagsWeight { get; set; }
        public decimal TotalWeight { get; set; }
    }

    public class up_GetFeedBillingReportByFeedId {
        public int FeedId { get; set; }
        public string TOTALPriceString { get; set; }
        public string FeedTotalBagsString { get; set; }
        public string FeedBagsWeightString { get; set; }
        public string TotalWeightString { get; set; }
        public string FeedName { get; set; }
        public decimal TOTALPrice { get; set; }
        public int FeedTotalBags { get; set; }
        public int FeedBagsWeight { get; set; }
        public decimal TotalWeight { get; set; }
    }


    public class FishBillingDownloadInfo {
        public IList<FishSellingTotalBySellerID> TotalSellingInfo { get; set; }
        public IList<FishBillingHistoryBySellerId> BillingHistory { get; set; }
    }

    public class FishSellingTotalBySellerID {
        public string TotalSellPrice { get; set; }
        public string TotalSellInKG { get; set; }
        public string PricePerKg { get; set; }
        public string SellDate { get; set; }
        public string SellerName { get; set; }
        public string FishAmountPaid { get; set; }
        public string FishAmountDue { get; set; }
    }


    public class FishBillingHistoryBySellerId {
        public string FishBillingFishInKG { get; set; }
        public string FishBillingTotalPrice { get; set; }
        public string FishBillingPaidAmount { get; set; }
        public string FishBillingDueAmount { get; set; }
        public string SellDate { get; set; }
        public string FishBillingDate { get; set; }
        public string SellingFishName { get; set; }
        public int FishSellerId { get; set; }
        public string SellerName { get; set; }
    }


}
