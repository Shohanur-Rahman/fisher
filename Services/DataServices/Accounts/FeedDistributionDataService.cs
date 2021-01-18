using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models.FeedDistribute;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Accounts
{
    public class FeedDistributionDataService : EntityContextDataService<FeedDistributionTableDoamain>
    {
        
        public FeedDistributionDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public IList<CreateSegmentFeed> GetFeedAllList()
        {
            List<CreateSegmentFeed> feedList = DbContext.Database.SqlQuery<CreateSegmentFeed>("SELECT * FROM Feed").ToList();
            return feedList;
        }

        public List<object> GetAllFeedCategories()
        {

            List<AllFeedCategories> temList = new List<AllFeedCategories>();
            List<object> myData = new List<object>();

            IList<CreateSegmentFeed> feedList = DbContext.Database.SqlQuery<CreateSegmentFeed>("SELECT * FROM Feed").ToList();
            if (feedList.Count > 0) {
                for (int i = 0; i < feedList.Count; i++) {
                    IList<CreateSegmentFeedCategories> feedCategoryList = new List<CreateSegmentFeedCategories>();
                    feedCategoryList = DbContext.Database.SqlQuery<CreateSegmentFeedCategories>("SELECT * FROM FeedCategories where FeedCategoryFeedId = " + feedList[i].ID + ";").ToList();
                    myData.Add(feedCategoryList);
                }
            }
            return myData;
        }



        public IList<CreateSegmentFeedCategories> GetFeedCategoryListByFeedId(int feedId)
        {
            List<CreateSegmentFeedCategories> feedCategoryList = DbContext.Database.SqlQuery<CreateSegmentFeedCategories>("SELECT * FROM FeedCategories where FeedCategoryFeedId = " + feedId  + ";").ToList();
            return feedCategoryList;
        }


        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributionReportsBySearchParam(string areaId, string projectId, string feedId, string categoryId, string calculationName)
        {

            SqlParameter prmCalculationName = new SqlParameter("@calculationName", calculationName);
            SqlParameter prmFeedId = new SqlParameter("@feedId", feedId);
            SqlParameter prmCategoryId = new SqlParameter("@categoryId", categoryId);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", areaId);

            List<SPFeedDistributionReportsBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFeedDistributionReportsBySearchParam>("up_GetFeedDistributionReportsBySearchParam @feedId,@categoryId,@areaId,@projectId,@calculationName", prmFeedId, prmCategoryId,prmAreaId,prmProjectId, prmCalculationName).ToList();

            return fishSellingReportList;
        }

        public bool ProcessFeedDistributionReports(FeedDistributionTableDataModal feedDistributionTableDataModal, int adminUserId)
        {
            
            if (feedDistributionTableDataModal != null && feedDistributionTableDataModal.isAddOperation == true)
            {
                FeedDistributionTableDoamain item = new FeedDistributionTableDoamain();
                item.FeedDistributionReportId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionReportId;
                item.FeedDistributionName = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionName;
                item.FeedDistributionDate = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionDate;
                //item.FeedDistributionFeedId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionFeedId;
                //item.FeedDistributionFeedategoryId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionFeedategoryId;
                //item.FeedDistributionQuantityId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionQuantityId;
                //item.FeedDistributionSakNumber = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionSakNumber;
                item.FeedDistributionTotalWeight = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionTotalWeight;
                item.FeedDistributionDescription = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionDescription;
                item.AreaId = feedDistributionTableDataModal.feedDistributionTableDoamain.AreaId;
                item.ProjectId = feedDistributionTableDataModal.feedDistributionTableDoamain.ProjectId;
                //item.IsActive = feedDistributionTableDataModal.feedDistributionTableDoamain.IsActive;

                item.CreatedDate = DateTime.Now;
                item.CreatedId = adminUserId;
                DbContext.Set<FeedDistributionTableDoamain>().Add(item);
                DbContext.SaveChanges();

                return true;
            }

            if (feedDistributionTableDataModal != null && feedDistributionTableDataModal.isAddOperation == false)
            {
                FeedDistributionTableDoamain item = DbContext.Set<FeedDistributionTableDoamain>().Where(x => x.FeedDistributionReportId == feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionReportId).FirstOrDefault();
                
                item.FeedDistributionName = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionName;
                item.FeedDistributionDate = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionDate;
                //item.FeedDistributionFeedId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionFeedId;
                //item.FeedDistributionFeedategoryId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionFeedategoryId;
                //item.FeedDistributionQuantityId = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionQuantityId;
                //item.FeedDistributionSakNumber = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionSakNumber;
                item.FeedDistributionTotalWeight = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionTotalWeight;
                item.FeedDistributionDescription = feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionDescription;
                item.AreaId = feedDistributionTableDataModal.feedDistributionTableDoamain.AreaId;
                item.ProjectId = feedDistributionTableDataModal.feedDistributionTableDoamain.ProjectId;

                item.EditedDate = DateTime.Now;
                item.EditedId = adminUserId;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }

        public FeedDistributionTableDoamain GetFeedDistributionDetailsById(int id)
        {
            FeedDistributionTableDoamain result = DbContext.Database.SqlQuery<FeedDistributionTableDoamain>("SELECT * FROM FeedDistributionReports where FeedDistributionReportId = " + id).FirstOrDefault();
            return result;
        }


        public int DeleteFeedDistributionRecord(int id)
        {
            int result = DbContext.Database.ExecuteSqlCommand("Delete from FeedDistributionReports where FeedDistributionReportId = " + id);
            return result;
        }

        public IList<SPFeedDistributionReportsBySearchParam> GetFeedDistributeReportByAreaId(string areaId, string projectId)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);

            List<SPFeedDistributionReportsBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFeedDistributionReportsBySearchParam>("up_GetFeedDistributionReportsByAreaId @areaId,@projectId", prmAreaId, prmProjectId).ToList();

            return fishSellingReportList;
        }

        public IList<up_GetFeedDistributionPieChart> GetFeedDistributionPieChart()
        {
            List<up_GetFeedDistributionPieChart> result = DbContext.Database.SqlQuery<up_GetFeedDistributionPieChart>("up_GetFeedDistributionPieChart").ToList();

            return result;
        }


    }
}
