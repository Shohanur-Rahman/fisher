using Services.ApplicationEntity.UserBilling;
using Services.Domain.UserBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.UserBilling
{
    public class UserBillingApplicationService
    {
        protected UserBillingAppEntity _userBillingAppEntityInstance { get; set; }

        public UserBillingApplicationService(UserBillingAppEntity userBillingAppEntity)
        {
            _userBillingAppEntityInstance = userBillingAppEntity;
        }

        public FishSellingDueBillingInfo GetFishSellingListForBilling(string isPartial, string sellerID)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetFishSellingListForBilling(isPartial, sellerID);
        }
        public BillingAmountHistoryBySellerId GetTotalAmountPaidAmountAndDueAmount(string fishSellId)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetTotalAmountPaidAmountAndDueAmount(fishSellId);
        }

        public bool UpdateFishSellingBilling(string fishSellerId, string paidAmount,int adminUserId)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.UpdateFishSellingBilling(fishSellerId, paidAmount, adminUserId);
        }

        public IList<FishSellingBillingHistoryList> GetBillingFishSellingSearchResult(string sellerId, string startDate, string endDate, string isPartial)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetBillingFishSellingSearchResult(sellerId, startDate, endDate, isPartial);
        }

        public BillingAmountHistoryBySellerId GetFishSellingFullReportBySellerId(string fishSellId)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetFishSellingFullReportBySellerId(fishSellId);
        }

        public FeedBillingReport GetFeedBillingReportByFeedId(string feedId)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetFeedBillingReportByFeedId(feedId);
        }

        public FishBillingDownloadInfo GetFishSellingTotalBySellerId(int sellerId)
        {
            return _userBillingAppEntityInstance.userBillingDomainService.GetFishSellingTotalBySellerId(sellerId);
        }

    }
}
