using Core.DomainService;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.FeedPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.Accounts
{ 
   public class FeedSellingDomainServices : DomainService<FeedSellingReportTableDomain, long>
    {
        private readonly FeedSellingReportDataService _feedSellingReportDataService;
        public FeedSellingDomainServices(FeedSellingReportDataService feedSellingReportDataService) : base(feedSellingReportDataService)
        {
            _feedSellingReportDataService = feedSellingReportDataService;
        }

        public IList<CreateSegmentFeedCategories> GetAllFeedCategoryList()
        {
            return _feedSellingReportDataService.GetAllFeedCategoryList();
        }

        public IList<CreateSegmentFeed> GetAllFeedNameList()
        {
            return _feedSellingReportDataService.GetAllFeedNameList();
        }

        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            return _feedSellingReportDataService.GetFeedCategoryListByFeedId(feedId);
        }
        public IList<SPFeedSellingReportsBySearchParam> GetFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial, int areaId, int projectId)
        {
            return _feedSellingReportDataService.GetFeedSellingReportBySearchParams(startDate, endDate, catId, feedId, isPartial, areaId, projectId);
        }

        public IList<SPTotalFeedSellingReportsBySearchParam> GetTotalFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial, int areaId, int projectId)
        {
            return _feedSellingReportDataService.GetTotalFeedSellingReportBySearchParams(startDate, endDate, catId, feedId, isPartial, areaId, projectId);
        }


        public bool ProcessFeedSellingReport(FeedSellingReportTableDataModal feedSellingReportTableDataModal, int adminUserId)
        {
            return _feedSellingReportDataService.ProcessFeedSellingReport(feedSellingReportTableDataModal, adminUserId);
        }

        public FeedSellingReportTableDomain GetFeedSellingReportDetailsById(int feedId)
        {
            return _feedSellingReportDataService.GetFeedSellingReportDetailsById(feedId);
        }

        public int DeleteFeedSellingRecord(int id)
        {
            return _feedSellingReportDataService.DeleteFeedSellingRecord(id);
        }

        public IList<SPFeedSellingUserDashboardOverview> GetFeedSellingDashboardOverview(int areaId, int projectId)
        {
            return _feedSellingReportDataService.GetFeedSellingDashboardOverview(areaId, projectId);
        }

        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategory(int areaId, int projectId)
        {
            return _feedSellingReportDataService.GetFeedBuyingReportGroupByFeedCategory(areaId, projectId);
        }


        public IList<SPFeedSellingUserDashboardOverview> GetFeedBuyingChartForAdmin()
        {
            return _feedSellingReportDataService.GetFeedBuyingChartForAdmin();
        }

        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategoryForAdmin()
        {
            return _feedSellingReportDataService.GetFeedBuyingReportGroupByFeedCategoryForAdmin();
        }

        public IList<up_GetFeedPurchaseReportByAreaId> GetFeedPurchaseReportByAreaId(string areaId)
        {
            return _feedSellingReportDataService.GetFeedPurchaseReportByAreaId(areaId);
        }
        public FirstPageFeedPurchaseTableTotal FirstPageGetFeedPurchaseReportByAreaId(string areaId)
        {
            return _feedSellingReportDataService.FirstPageGetFeedPurchaseReportByAreaId(areaId);
        }


        


    }
}
