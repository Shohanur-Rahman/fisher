using Core.DomainService;
using Services.DataServices.CreateSegement.Fish;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.CreateSegement.Fish
{

    public class FishSegmentDomainService : DomainService<CreateSegmentFish, long> 
    {

        private readonly FishSegmentDataService _fishSegmentDataService;
        public FishSegmentDomainService(FishSegmentDataService fishSegmentDomainService) : base(fishSegmentDomainService)
        {
            _fishSegmentDataService = fishSegmentDomainService;
        }

        // All region for Segment Fish

        #region "Segment Fish  ================================================================="



        #region "Process segment fish"
        public bool ProcessSegmentFish(CreateSegmentFishModel createSegmentFishModel, int adminUserId)
        {
            return _fishSegmentDataService.ProcessSegmentFish(createSegmentFishModel, adminUserId);
        }

        public bool ProcessSegmentFishSeller(CreateSegmentFishSellerModel createSegmentFishModel, int adminUserId)
        {
            return _fishSegmentDataService.ProcessSegmentFishSeller(createSegmentFishModel, adminUserId);
        }


        #endregion

        #region "Get Segment Fish list"
        public IList<CreateSegmentFishList> GetSegmentFishListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return _fishSegmentDataService.GetSegmentFishListBySearch(searchText, pageNumber, pageSize);
        }


        public IList<SPSegmentFishSellerList> GetSegmentFishSellerListBySearch(string searchText)
        {
            return _fishSegmentDataService.GetSegmentFishSellerListBySearch(searchText);
        }

        #endregion

        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryList(string searchText, int pageNumber, int pageSize)
        {
            return _fishSegmentDataService.GetSegmentFeedCategoryList(searchText, pageNumber, pageSize);
        }

        #region "Get Segment Fish Info"
        public CreateSegmentFish GetSegmentFishDetailsByFishId(int fishID)
        {
            return _fishSegmentDataService.GetSegmentFishDetailsByFishId(fishID); 
        }

        public CreateSegmentFishSeller GetSegmentFishSellerDetailsByFishId(int fishID)
        {
            return _fishSegmentDataService.GetSegmentFishSellerDetailsByFishId(fishID);
        }

        #endregion


        #endregion


        public int DeleteFish(int id)
        {
            return _fishSegmentDataService.DeleteFish(id);
        }

        public int DeleteFishSeller(int id)
        {
            return _fishSegmentDataService.DeleteFishSeller(id);
        }

    }
}
