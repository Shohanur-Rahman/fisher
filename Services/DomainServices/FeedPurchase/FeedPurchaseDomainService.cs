using Core.DomainService;
using Services.DataServices.FeedPurchase;
using Services.Domain.FeedPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.FeedPurchase
{
    public class FeedPurchaseDomainService : DomainService<FeedPurchaseTableDomain, long>
    {
        private readonly FeedPurchaseDataService _feedPurchaseDataService;
        public FeedPurchaseDomainService(FeedPurchaseDataService feedPurchaseDataService) : base(feedPurchaseDataService)
        {
            _feedPurchaseDataService = feedPurchaseDataService;
        }

        public bool ProcessFeedPurchaseReport(FeedPurchaseTableDomain feedPurchaseReport, int adminUserId)
        {
            return _feedPurchaseDataService.ProcessFeedPurchaseReport(feedPurchaseReport, adminUserId);
        }

        public FeedPurchaseSearchResultByParam GetFeedPurchaseSearchResultByParam(string areaId, string projectId, string feedId, string catId, string calculationName)
        {
            return _feedPurchaseDataService.GetFeedPurchaseSearchResultByParam(areaId, projectId, feedId, catId, calculationName);
        }

        public FeedPurchaseTableDomain GetFeedPurchaseDetailsByFeedPurchaseID(int id)
        {
            return _feedPurchaseDataService.GetFeedPurchaseDetailsByFeedPurchaseID(id);
        }

        public int DeleteFeedPurchaseRecord(int id)
        {
            return _feedPurchaseDataService.DeleteFeedPurchaseRecord(id);
        }

        public FeedPurchasePDFInfo GetSingleFeedPurchaseReport(int purchaseId)
        {
            return _feedPurchaseDataService.GetSingleFeedPurchaseReport(purchaseId);
        }

        public IList<up_GetFeedPurchasePieChart> GetFeedPurchasePieChart()
        {
            return _feedPurchaseDataService.GetFeedPurchasePieChart();
        }

    }
}
