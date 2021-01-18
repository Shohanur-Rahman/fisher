using Core.DomainService;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.FeedDistribute;
using Services.Domain.CreateSegement.Models.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.Accounts
{ 
    public class FeedDistributionDoamainService : DomainService<FeedDistributionTableDoamain, long>
    { 
        private readonly FeedDistributionDataService _feedDistributionDataService;
        public FeedDistributionDoamainService(FeedDistributionDataService feedDistributionDataService) : base(feedDistributionDataService)
        {
            _feedDistributionDataService = feedDistributionDataService;
        }

        public IList<CreateSegmentFeed> GetFeedAllList()
        {
            return _feedDistributionDataService.GetFeedAllList();
        }
        public List<object> GetAllFeedCategories()
        {
            return _feedDistributionDataService.GetAllFeedCategories();
        }


        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            return _feedDistributionDataService.GetFeedCategoryListByFeedId(feedId);
        }

        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributionReportsBySearchParam(string areaId, string projectId, string feedId, string categoryId, string calculationName)
        {
            return _feedDistributionDataService.GetFeedDistributionReportsBySearchParam(areaId, projectId, feedId, categoryId, calculationName);
        }



        public bool ProcessFeedDistributionReports(FeedDistributionTableDataModal feedDistributionTableDataModal, int adminUserId)
        {
            return _feedDistributionDataService.ProcessFeedDistributionReports(feedDistributionTableDataModal, adminUserId);
        }

        public FeedDistributionTableDoamain GetFeedDistributionDetailsById(int id)
        {
            return _feedDistributionDataService.GetFeedDistributionDetailsById(id);
        }


        public int DeleteFeedDistributionRecord(int id)
        {
            return _feedDistributionDataService.DeleteFeedDistributionRecord(id);
        }

        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributeReportByAreaId(string areaId, string projectId)
        {
            return _feedDistributionDataService.GetFeedDistributeReportByAreaId(areaId,projectId);
        }

        public IList<up_GetFeedDistributionPieChart> GetFeedDistributionPieChart()
        {
            return _feedDistributionDataService.GetFeedDistributionPieChart();
        }
    }
}
