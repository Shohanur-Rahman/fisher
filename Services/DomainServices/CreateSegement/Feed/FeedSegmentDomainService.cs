using Core.DomainService;
using Services.DataServices.CreateSegement.Feed;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.CreateSegement.Feed
{
    public class FeedSegmentDomainService : DomainService<CreateSegmentFeed, long>
    {
        private readonly FeedSegmentDataService _feedSegmentDataService;
        public FeedSegmentDomainService(FeedSegmentDataService feedSegmentDomainService) : base(feedSegmentDomainService)
        {
            _feedSegmentDataService = feedSegmentDomainService;
        }

        // All region for Segment Feed

        #region "Segment Feed"

        #region "Process segment feed"
        public bool ProcessSegmentFeed(CreateSegmentFeedModel createSegmentFeedhModel, int adminUserId)
        {
            return _feedSegmentDataService.ProcessSegmentFeed(createSegmentFeedhModel, adminUserId);
        }

        public bool ProcessSegmentFeedCategory(CreateFeedSegmentCategoriesModal feedCategoryModal, int adminUserId)
        {
            return _feedSegmentDataService.ProcessSegmentFeedCategory(feedCategoryModal, adminUserId);
        }


        #endregion

        #region "Get Segment Feed list"
        public IList<CreateSegmentFeedList> GetSegmentFeedListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return _feedSegmentDataService.GetSegmentFeedListBySearch(searchText, pageNumber, pageSize);
        }

        public IList<CreateSegmentFeed> GetFeedListForCategory()
        {
            return _feedSegmentDataService.GetFeedListForCategory();
        }
        #endregion


        #region "Get Segment Feed Info"
        public CreateSegmentFeed GetSegmentFeedDetailsByFishId(int feedId)
        {
            return _feedSegmentDataService.GetSegmentFeedDetailsByFishId(feedId);
        }
        public CreateSegmentFeedCategories GetSegmentFeedCategoryDetails(int feedId)
        {
            return _feedSegmentDataService.GetSegmentFeedCategoryDetails(feedId);
        }
        #endregion

        #endregion

        public int DeleteFeed(int id)
        {
            return _feedSegmentDataService.DeleteFeed(id);
        }

        public int DeleteFeedCategory(int id)
        {
            return _feedSegmentDataService.DeleteFeedCategory(id);
        }


        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryDataList(string searchText, int pageNumber, int pageSize)
        {
            return _feedSegmentDataService.GetSegmentFeedCategoryDataList(searchText, pageNumber, pageSize);
        }
    }
}
