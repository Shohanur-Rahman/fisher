using Services.ApplicationEntity.CreateSegement;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.CreateSegement.Feed
{
    public class FeedSegmentApplicationService
    {
        protected CreateSegementAppEntity CreateSegementAppEntityInstance { get; set; }

        public FeedSegmentApplicationService(CreateSegementAppEntity createSegementAppEntity)
        {
            CreateSegementAppEntityInstance = createSegementAppEntity;
        }






        // All region for Segment Feed

        #region "Segment Feed ==============================================================================="


        #region "Process Feed Segment"
        public bool ProcessSegmentFeed(CreateSegmentFeedModel createSegmentFeedhModel, int adminUserId)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.ProcessSegmentFeed(createSegmentFeedhModel, adminUserId);
        }



        public bool ProcessSegmentFeedCategory(CreateFeedSegmentCategoriesModal feedCategoryModal, int adminUserId)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.ProcessSegmentFeedCategory(feedCategoryModal, adminUserId);
        }


        #endregion


        #region "Get Fed List"
        public IList<CreateSegmentFeedList> GetSegmentFeedListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.GetSegmentFeedListBySearch(searchText, pageNumber, pageSize);
        }

        public IList<CreateSegmentFeed> GetFeedListForCategory()
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.GetFeedListForCategory();
        }

        #endregion

        #region "Get Feed Details"
        public CreateSegmentFeed GetSegmentFeedDetailsByFishId(int feedId)
        {
            CreateSegmentFeed createSegmentFeed = CreateSegementAppEntityInstance.feedSegmentDomainService.GetSegmentFeedDetailsByFishId(feedId);
            if (createSegmentFeed != null)
            {
                return createSegmentFeed;

            }
            return null;
        }

        public CreateSegmentFeedCategories GetSegmentFeedCategoryDetails(int feedId)
        {
            CreateSegmentFeedCategories createSegmentFeed = CreateSegementAppEntityInstance.feedSegmentDomainService.GetSegmentFeedCategoryDetails(feedId);
            if (createSegmentFeed != null)
            {
                return createSegmentFeed;

            }
            return null;
        }
        #endregion

        #endregion

        public int DeleteFeed(int id)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.DeleteFeed(id);
        }


        public int DeleteFeedCategory(int id)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.DeleteFeedCategory(id);
        }

        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryDataList(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.feedSegmentDomainService.GetSegmentFeedCategoryDataList(searchText, pageNumber, pageSize);
        }

    }
}
