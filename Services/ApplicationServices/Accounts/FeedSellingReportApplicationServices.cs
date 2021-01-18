using Services.ApplicationEntity.Accounts;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.FeedPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Accounts
{
    public class FeedSellingReportApplicationServices
    {
        protected FeedSellingReportAppEntity _FeedSellingReportAppEntityAppEntityInstance { get; set; }

        public FeedSellingReportApplicationServices(FeedSellingReportAppEntity feedSellingReportAppEntity)
        {
            _FeedSellingReportAppEntityAppEntityInstance = feedSellingReportAppEntity;
        }


        public IList<CreateSegmentFeedCategories> GetAllFeedCategoryList()
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetAllFeedCategoryList();
        }

        public IList<CreateSegmentFeed> GetAllFeedNameList()
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetAllFeedNameList();
        }
        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedCategoryListByFeedId(feedId);
        }

        public IList<SPFeedSellingReportsBySearchParam> GetFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial,int areaId,int projectId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedSellingReportBySearchParams(startDate,endDate, catId, feedId, isPartial,areaId,projectId);
        }

        public IList<SPTotalFeedSellingReportsBySearchParam> GetTotalFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial, int areaId, int projectId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetTotalFeedSellingReportBySearchParams(startDate, endDate, catId, feedId, isPartial, areaId, projectId);
        }


        public bool ProcessFeedSellingReport(FeedSellingReportTableDataModal feedSellingReportTableDataModal, int adminUserId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.ProcessFeedSellingReport(feedSellingReportTableDataModal, adminUserId);
        }

        public FeedSellingReportTableDomain GetFeedSellingReportDetailsById(int feedId)
        {
            FeedSellingReportTableDomain sellingReport = _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedSellingReportDetailsById(feedId);
            if (sellingReport != null)
            {
                return sellingReport;

            }
            return null;
        }

        public int DeleteFeedSellingRecord(int id)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.DeleteFeedSellingRecord(id);
        }


        public IList<SPFeedSellingUserDashboardOverview> GetFeedSellingDashboardOverview(int areaId, int projectId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedSellingDashboardOverview(areaId, projectId);
        }


        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategory(int areaId, int projectId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedBuyingReportGroupByFeedCategory(areaId, projectId);
        }
        public IList<SPFeedSellingUserDashboardOverview> GetFeedBuyingChartForAdmin()
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedBuyingChartForAdmin();
        }

        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategoryForAdmin()
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedBuyingReportGroupByFeedCategoryForAdmin();
        }

        public IList<up_GetFeedPurchaseReportByAreaId> GetFeedPurchaseReportByAreaId(string areaId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.GetFeedPurchaseReportByAreaId(areaId);
        }

        public FirstPageFeedPurchaseTableTotal FirstPageGetFeedPurchaseReportByAreaId(string areaId)
        {
            return _FeedSellingReportAppEntityAppEntityInstance.feedSellingDomainServices.FirstPageGetFeedPurchaseReportByAreaId(areaId);
        }


    }
}
