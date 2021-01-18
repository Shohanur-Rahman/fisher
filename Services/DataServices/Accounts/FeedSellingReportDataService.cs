using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Utilities;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.CreateSegement.Models.Feed;
using System.Data.SqlClient;
using Services.Domain.FeedPurchase;

namespace Services.DataServices.Accounts
{
    public class FeedSellingReportDataService: EntityContextDataService<FeedSellingReportTableDomain>
    {
        public FeedSellingReportDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public IList<CreateSegmentFeedCategories> GetAllFeedCategoryList()
        {
            List<CreateSegmentFeedCategories> sellingReportList = DbContext.Database.SqlQuery<CreateSegmentFeedCategories>("SELECT * FROM FeedCategories").ToList();

            return sellingReportList;
        }

        public IList<CreateSegmentFeed> GetAllFeedNameList()
        {
            List<CreateSegmentFeed> nameList = DbContext.Database.SqlQuery<CreateSegmentFeed>("SELECT * FROM Feed").ToList();

            return nameList;
        }
        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            List<CreateSegmentFeedCategories> sellingReportList = DbContext.Database.SqlQuery<CreateSegmentFeedCategories>("SELECT * FROM FeedCategories where FeedCategoryFeedId = " + feedId + ";").ToList();

            return sellingReportList;
        }

        public IList<SPFeedSellingReportsBySearchParam> GetFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial, int areaId, int projectId)
        {

            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmCatId = new SqlParameter("@categoryId", catId);
            SqlParameter prmFeedId = new SqlParameter("@feedId", feedId);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);

            List<SPFeedSellingReportsBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFeedSellingReportsBySearchParam>("up_GetFeedSellingReportsBySearchParam @startDate,@endDate,@categoryId,@feedId,@isPartial,@areaId,@projectId", prmStartDate, prmEndDate, prmCatId, prmFeedId, prmIsPartial, prmAreaId, prmProjectId).ToList();

            return fishSellingReportList;
        }

        public IList<SPTotalFeedSellingReportsBySearchParam> GetTotalFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial, int areaId, int projectId)
        {

            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmCatId = new SqlParameter("@categoryId", catId);
            SqlParameter prmFeedId = new SqlParameter("@feedId", feedId);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);

            List<SPTotalFeedSellingReportsBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPTotalFeedSellingReportsBySearchParam>("up_GetTotalFeedSellingReportsBySearchParam @startDate,@endDate,@categoryId,@feedId,@isPartial,@areaId,@projectId", prmStartDate, prmEndDate, prmCatId, prmFeedId, prmIsPartial, prmAreaId, prmProjectId).ToList();

            return fishSellingReportList;
        }

        public bool ProcessFeedSellingReport(FeedSellingReportTableDataModal feedSellingReportTableDataModal, int adminUserId)
        {


            if (feedSellingReportTableDataModal != null && feedSellingReportTableDataModal.isAddOperation == true)
            {
                FeedSellingReportTableDomain sellingReport = new FeedSellingReportTableDomain();
                sellingReport.FeedSellingReportId = feedSellingReportTableDataModal.feedSellingReportTableDomain.FeedSellingReportId;
                sellingReport.SellingFeedReportNumber = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedReportNumber;
                sellingReport.FeedId = feedSellingReportTableDataModal.feedSellingReportTableDomain.FeedId;
                sellingReport.SellingReportName = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingReportName;
                sellingReport.SellingReportFeedCategoryId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingReportFeedCategoryId;
                sellingReport.SellingFeedAreaId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedAreaId;
                sellingReport.SellingFeedProjectId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedProjectId;
                sellingReport.SellingFeedWeightPerSack = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedWeightPerSack;
                sellingReport.SellingFeedSackNumber = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedSackNumber;
                sellingReport.SellingFeedTotalWeight = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedTotalWeight;
                sellingReport.SellingFeedPricePerKg = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedPricePerKg;
                sellingReport.SellingFeedTotalPrice = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedTotalPrice;
                sellingReport.SellingFeedCalculationDate = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedCalculationDate;
                sellingReport.SellingFeedSellNote = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedSellNote;

                sellingReport.SellingFeedCreateDate = DateTime.Now;
                sellingReport.SellingFeedCreatedId = adminUserId;
                DbContext.Set<FeedSellingReportTableDomain>().Add(sellingReport);
                DbContext.SaveChanges();

                return true;
            }

            if (feedSellingReportTableDataModal != null && feedSellingReportTableDataModal.isAddOperation == false)
            {
                FeedSellingReportTableDomain sellingReport = DbContext.Set<FeedSellingReportTableDomain>().Where(x => x.FeedSellingReportId == feedSellingReportTableDataModal.feedSellingReportTableDomain.FeedSellingReportId).FirstOrDefault();

                sellingReport.SellingFeedReportNumber = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedReportNumber;
                sellingReport.SellingReportName = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingReportName;
                sellingReport.FeedId = feedSellingReportTableDataModal.feedSellingReportTableDomain.FeedId;
                sellingReport.SellingReportFeedCategoryId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingReportFeedCategoryId;
                sellingReport.SellingFeedAreaId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedAreaId;
                sellingReport.SellingFeedProjectId = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedProjectId;
                sellingReport.SellingFeedWeightPerSack = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedWeightPerSack;
                sellingReport.SellingFeedSackNumber = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedSackNumber;
                sellingReport.SellingFeedTotalWeight = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedTotalWeight;
                sellingReport.SellingFeedPricePerKg = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedPricePerKg;
                sellingReport.SellingFeedTotalPrice = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedTotalPrice;
                sellingReport.SellingFeedCalculationDate = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedCalculationDate;
                sellingReport.SellingFeedSellNote = feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedSellNote;

                sellingReport.SellingFeedEditedDate = DateTime.Now;
                sellingReport.SellignFeedEditedId = adminUserId;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public FeedSellingReportTableDomain GetFeedSellingReportDetailsById(int feedId)
        {
            FeedSellingReportTableDomain sellingReport = new FeedSellingReportTableDomain();
            sellingReport = DbContext.Set<FeedSellingReportTableDomain>().Where(x => x.FeedSellingReportId == feedId).FirstOrDefault();
            return sellingReport;
        }

        public int DeleteFeedSellingRecord(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("DELETE FROM[FeedSellingReport] where FeedSellingReportId=" + id);

            return result;
        }


        public IList<SPFeedSellingUserDashboardOverview> GetFeedSellingDashboardOverview(int areaId, int projectId)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);

            List<SPFeedSellingUserDashboardOverview> result = DbContext.Database.SqlQuery<SPFeedSellingUserDashboardOverview>("up_GetFeedBuyingChartReportForUser @areaId,@projectId", prmAreaId, prmProjectId).ToList();

            return result;
        }



        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategory(int areaId, int projectId)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);

            List<SPFeedBuyingReportByFeedCategory> result = DbContext.Database.SqlQuery<SPFeedBuyingReportByFeedCategory>("up_GetFeedBuyingReportByFeedCategory @areaId,@projectId", prmAreaId, prmProjectId).ToList();

            return result;
        }

        public IList<SPFeedSellingUserDashboardOverview> GetFeedBuyingChartForAdmin()
        {
            List<SPFeedSellingUserDashboardOverview> result = DbContext.Database.SqlQuery<SPFeedSellingUserDashboardOverview>("up_GetFeedBuyingChartReportForAdmin").ToList();

            return result;
        }

        public IList<SPFeedBuyingReportByFeedCategory> GetFeedBuyingReportGroupByFeedCategoryForAdmin()
        {
            List<SPFeedBuyingReportByFeedCategory> result = DbContext.Database.SqlQuery<SPFeedBuyingReportByFeedCategory>("up_GetFeedBuyingReportByFeedCategoryForAdmin").ToList();

            return result;
        }

        public IList<up_GetFeedPurchaseReportByAreaId> GetFeedPurchaseReportByAreaId(string areaId)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);

            List<up_GetFeedPurchaseReportByAreaId> result = DbContext.Database.SqlQuery<up_GetFeedPurchaseReportByAreaId>("up_GetFeedPurchaseReportByAreaId @areaId", prmAreaId).ToList();

            return result;
        }
        public FirstPageFeedPurchaseTableTotal FirstPageGetFeedPurchaseReportByAreaId(string areaId)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);

            FirstPageFeedPurchaseTableTotal result = DbContext.Database.SqlQuery<FirstPageFeedPurchaseTableTotal>("up_GetTotalFeedPurchaseReportByAreaId @areaId", prmAreaId).FirstOrDefault();
            

            return result;
        }



    }
}
