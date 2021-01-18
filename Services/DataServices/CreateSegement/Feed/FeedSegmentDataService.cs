using Core.DataService;
using Services.DataContext;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.SPModels;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.CreateSegement.Feed
{
        public class FeedSegmentDataService : EntityContextDataService<CreateSegmentFeed>
        {
            public FeedSegmentDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }




        // All region for Segment Feed

        #region "Segment Feed  ======================================================================================"

        #region "Process Segment Feed"

        public bool ProcessSegmentFeed(CreateSegmentFeedModel createSegmentFeedhModel, int adminUserId)
        {


            if (createSegmentFeedhModel != null && createSegmentFeedhModel.isAddOperation == true)
            {
                CreateSegmentFeed segmentFeed = new CreateSegmentFeed();
                segmentFeed.ID = createSegmentFeedhModel.CreateSegmentFeed.ID;
                segmentFeed.Name = createSegmentFeedhModel.CreateSegmentFeed.Name;

                if (createSegmentFeedhModel.CreateSegmentFeed.ImageUrl != null)
                {
                    segmentFeed.ImageUrl = createSegmentFeedhModel.CreateSegmentFeed.ImageUrl;
                }
                else {
                    segmentFeed.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentFeed.Description = createSegmentFeedhModel.CreateSegmentFeed.Description;
                segmentFeed.IsDeleted = false;
                segmentFeed.CreatedDate = DateTime.Now;
                segmentFeed.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentFeed>().Add(segmentFeed); 
                DbContext.SaveChanges();

                return true;
            }

            if (createSegmentFeedhModel != null && createSegmentFeedhModel.isAddOperation == false)
            {
                CreateSegmentFeed segmentFeed = DbContext.Set<CreateSegmentFeed>().Where(x => x.ID == createSegmentFeedhModel.CreateSegmentFeed.ID).FirstOrDefault();

                segmentFeed.Name = createSegmentFeedhModel.CreateSegmentFeed.Name;

                if (createSegmentFeedhModel.CreateSegmentFeed.ImageUrl != null)
                {
                    segmentFeed.ImageUrl = createSegmentFeedhModel.CreateSegmentFeed.ImageUrl;
                }
                else {
                    segmentFeed.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentFeed.Description = createSegmentFeedhModel.CreateSegmentFeed.Description;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }



        public bool ProcessSegmentFeedCategory(CreateFeedSegmentCategoriesModal feedCategoryModal, int adminUserId)
        {


            if (feedCategoryModal != null && feedCategoryModal.isAddOperation == true)
            {
                CreateSegmentFeedCategories feedCategory = new CreateSegmentFeedCategories();
                feedCategory.FeedCategoryId = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryId;
                feedCategory.FeedCategoryName = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryName;

                if (feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryImageUrl != null)
                {
                    feedCategory.FeedCategoryImageUrl = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryImageUrl;
                }
                else
                {
                    feedCategory.FeedCategoryImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                feedCategory.FeedCategoryDetails = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryDetails == null ? "" : feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryDetails;
                feedCategory.FeedCategoryFeedId = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryFeedId;
                feedCategory.IsDeleted = false;
                feedCategory.CreatedDate = DateTime.Now;
                feedCategory.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentFeedCategories>().Add(feedCategory);
                DbContext.SaveChanges();

                return true;
            }

            if (feedCategoryModal != null && feedCategoryModal.isAddOperation == false)
            {
                CreateSegmentFeedCategories feedCategory = DbContext.Set<CreateSegmentFeedCategories>().Where(x => x.FeedCategoryId == feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryId).FirstOrDefault();

                feedCategory.FeedCategoryName = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryName;

                if (feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryImageUrl != null)
                {
                    feedCategory.FeedCategoryImageUrl = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryImageUrl;
                }
                else
                {
                    feedCategory.FeedCategoryImageUrl = "assets/global/img/rams-logo.jpeg";
                }
                feedCategory.FeedCategoryFeedId = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryFeedId;
                feedCategory.FeedCategoryDetails = feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryDetails == null ? "" : feedCategoryModal.CreateSegmentFeedCategory.FeedCategoryDetails;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }

        #endregion



        #region "Get Segment Fish List"
        public IList<CreateSegmentFeedList> GetSegmentFeedListBySearch(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<CreateSegmentFeedList> segmentFeedList = DbContext.Database.SqlQuery<CreateSegmentFeedList>("up_GetSegmentFeedListBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return segmentFeedList;
        }
        #endregion


        #region "Get Segment Fish Info"
        public CreateSegmentFeed GetSegmentFeedDetailsByFishId(int feedId)
        {
            CreateSegmentFeed feedDetails = new CreateSegmentFeed();
            feedDetails = DbContext.Set<CreateSegmentFeed>().Where(x => x.ID == feedId).FirstOrDefault();
            return feedDetails;
        }

        public CreateSegmentFeedCategories GetSegmentFeedCategoryDetails(int feedId)
        {
            CreateSegmentFeedCategories feedDetails = new CreateSegmentFeedCategories();
            feedDetails = DbContext.Set<CreateSegmentFeedCategories>().Where(x => x.FeedCategoryId == feedId).FirstOrDefault();
            return feedDetails;
        }
        #endregion


        #endregion

        public IList<CreateSegmentFeed> GetFeedListForCategory()
        {
            IList<CreateSegmentFeed> result = DbContext.Database.SqlQuery<CreateSegmentFeed>("SELECT * FROM Feed").ToList();
            return result;
        }

        public int DeleteFeed(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("update [Feed] set IsDeleted = 1 where id=" + id);

            return result;
        }

        public int DeleteFeedCategory(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("update [FeedCategories] set IsDeleted = 1 where FeedCategoryId=" + id);

            return result;
        }

        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryDataList(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<SPCreateSegmentFeedCategoryList> segmentFeedList = DbContext.Database.SqlQuery<SPCreateSegmentFeedCategoryList>("up_GetSegmentFeedCategoryListBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return segmentFeedList;
        }

    }
}
