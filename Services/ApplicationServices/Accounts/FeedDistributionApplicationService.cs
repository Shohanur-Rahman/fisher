using Services.ApplicationEntity.Accounts;
using Services.Domain.Accounts.Models.FeedDistribute;
using Services.Domain.CreateSegement.Models.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Accounts
{
    public class FeedDistributionApplicationService
    {
        protected FeedDistributionAppEntity _feedDistributionAppEntityInstance { get; set; }

        public FeedDistributionApplicationService(FeedDistributionAppEntity feedDistributionAppEntity)
        {
            _feedDistributionAppEntityInstance = feedDistributionAppEntity;
        }

        public IList<CreateSegmentFeed> GetFeedAllList() {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedAllList();
        }
        public List<object> GetAllFeedCategories()
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetAllFeedCategories();
        }

        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedCategoryListByFeedId(feedId);
        }

        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributionReportsBySearchParam(string areaId, string projectId, string feedId, string categoryId, string calculationName)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedDistributionReportsBySearchParam(areaId, projectId, feedId, categoryId, calculationName);
        }

        public bool ProcessFeedDistributionReports(FeedDistributionTableDataModal feedDistributionTableDataModal, int adminUserId)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.ProcessFeedDistributionReports(feedDistributionTableDataModal, adminUserId);
        }
        public FeedDistributionTableDoamain GetFeedDistributionDetailsById(int id)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedDistributionDetailsById(id);
        }

        public int DeleteFeedDistributionRecord(int id)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.DeleteFeedDistributionRecord(id);
        }

        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributeReportByAreaId(string areaId, string projectId)
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedDistributeReportByAreaId(areaId,projectId);
        }

        public IList<up_GetFeedDistributionPieChart> GetFeedDistributionPieChart()
        {
            return _feedDistributionAppEntityInstance.feedDistributionDoamainServices.GetFeedDistributionPieChart();
        }

    }
}
