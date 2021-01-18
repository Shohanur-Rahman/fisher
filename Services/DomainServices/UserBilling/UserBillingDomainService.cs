using Core.DomainService;
using Services.DataServices.UserBilling;
using Services.Domain.UserBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.UserBilling
{
    public class UserBillingDomainService : DomainService<FishListForUserBilling, long>
    {
        private readonly UserBillingDataService _userBillingDataService;
        public UserBillingDomainService(UserBillingDataService userBillingDataService) : base(userBillingDataService)
        {
            _userBillingDataService = userBillingDataService;
        }

        public FishSellingDueBillingInfo GetFishSellingListForBilling(string isPartial, string sellerID)
        {
            return _userBillingDataService.GetFishSellingListForBilling(isPartial, sellerID);
        }

        public BillingAmountHistoryBySellerId GetTotalAmountPaidAmountAndDueAmount(string fishSellId)
        {
            return _userBillingDataService.GetTotalAmountPaidAmountAndDueAmount(fishSellId);
        }

        public bool UpdateFishSellingBilling(string fishSellerId, string paidAmount,int adminUserId)
        {
            return _userBillingDataService.UpdateFishSellingBilling(fishSellerId, paidAmount, adminUserId);
        }
        public IList<FishSellingBillingHistoryList> GetBillingFishSellingSearchResult(string sellerId, string startDate, string endDate, string isPartial)
        {
            return _userBillingDataService.GetBillingFishSellingSearchResult(sellerId, startDate, endDate, isPartial);
        }
        public BillingAmountHistoryBySellerId GetFishSellingFullReportBySellerId(string fishSellId)
        {
            return _userBillingDataService.GetFishSellingFullReportBySellerId(fishSellId);
        }

        public FeedBillingReport GetFeedBillingReportByFeedId(string feedId)
        {
            return _userBillingDataService.GetFeedBillingReportByFeedId(feedId);
        }

        public FishBillingDownloadInfo GetFishSellingTotalBySellerId(int sellerId)
        {
            return _userBillingDataService.GetFishSellingTotalBySellerId(sellerId);
        }

    }
}
