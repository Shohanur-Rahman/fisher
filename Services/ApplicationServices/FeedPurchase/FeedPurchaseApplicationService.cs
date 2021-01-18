using Services.ApplicationEntity.FeedPurchase;
using Services.Domain.FeedPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.FeedPurchase
{
    public class FeedPurchaseApplicationService
    {
        protected FeedPurchaseAppEntity _feedPurchaseAppEntityInstance { get; set; }

        public FeedPurchaseApplicationService(FeedPurchaseAppEntity feedPurchaseAppEntity)
        {
            _feedPurchaseAppEntityInstance = feedPurchaseAppEntity;
        }

        public bool ProcessFeedPurchaseReport(FeedPurchaseTableDomain feedPurchaseReport, int adminUserId)
        {
            return _feedPurchaseAppEntityInstance.feedPurchaseDomainService.ProcessFeedPurchaseReport(feedPurchaseReport, adminUserId);
        }

        public FeedPurchaseSearchResultByParam GetFeedPurchaseSearchResultByParam(string areaId, string projectId, string feedId, string catId, string calculationName)
        {
            return _feedPurchaseAppEntityInstance.feedPurchaseDomainService.GetFeedPurchaseSearchResultByParam(areaId, projectId, feedId, catId, calculationName);
        }

        public FeedPurchaseTableDomain GetFeedPurchaseDetailsByFeedPurchaseID(int id)
        {
            FeedPurchaseTableDomain sellingReport = _feedPurchaseAppEntityInstance.feedPurchaseDomainService.GetFeedPurchaseDetailsByFeedPurchaseID(id);
            if (sellingReport != null)
            {
                return sellingReport;

            }
            return null;
        }

        public int DeleteFeedPurchaseRecord(int id)
        {
            return _feedPurchaseAppEntityInstance.feedPurchaseDomainService.DeleteFeedPurchaseRecord(id);
        }

        public FeedPurchasePDFInfo GetSingleFeedPurchaseReport(int purchaseId)
        {
            return _feedPurchaseAppEntityInstance.feedPurchaseDomainService.GetSingleFeedPurchaseReport(purchaseId);
        }

        public IList<up_GetFeedPurchasePieChart> GetFeedPurchasePieChart()
        {
            return _feedPurchaseAppEntityInstance.feedPurchaseDomainService.GetFeedPurchasePieChart();
        }

    }
}
