using Services.ApplicationEntity.CreateSegement;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.CreateSegement.Fish
{
    public class FishSegmentApplicationService
    {
        protected CreateSegementAppEntity CreateSegementAppEntityInstance { get; set; }
         
        public FishSegmentApplicationService(CreateSegementAppEntity createSegementAppEntity)
        {
            CreateSegementAppEntityInstance = createSegementAppEntity;
        }
        // All region for Segment Fish

        #region "Segment Fish ================================================================================="


        #region "Process Fish Segment"
        public bool ProcessSegmentFish(CreateSegmentFishModel createSegmentFishModel, int adminUserId)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.ProcessSegmentFish(createSegmentFishModel, adminUserId);
        }

        public bool ProcessSegmentFishSeller(CreateSegmentFishSellerModel createSegmentFishModel, int adminUserId)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.ProcessSegmentFishSeller(createSegmentFishModel, adminUserId);
        }

        #endregion


        #region "Get Fish List"
        public IList<CreateSegmentFishList> GetSegmentFishListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.GetSegmentFishListBySearch(searchText, pageNumber, pageSize);
        }


        public IList<SPSegmentFishSellerList> GetSegmentFishSellerListBySearch(string searchText)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.GetSegmentFishSellerListBySearch(searchText);
        }

        #endregion

        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryList(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.GetSegmentFeedCategoryList(searchText, pageNumber, pageSize);
        }



        #region "Get Fish Details"
        public CreateSegmentFishSeller GetSegmentFishSellerDetailsByFishId(int fishID)
        {
            CreateSegmentFishSeller fishSeller = CreateSegementAppEntityInstance.fishSegmentDomainService.GetSegmentFishSellerDetailsByFishId(fishID);
            if (fishSeller != null)
            {
                return fishSeller; 

            }
            return null;
        }

        public CreateSegmentFish GetSegmentFishDetailsByFishId(int fishID)
        {
            CreateSegmentFish createSegmentFish = CreateSegementAppEntityInstance.fishSegmentDomainService.GetSegmentFishDetailsByFishId(fishID);
            if (createSegmentFish != null)
            {
                return createSegmentFish;

            }
            return null;
        }


        #endregion

        #endregion

        public int DeleteFish(int id)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.DeleteFish(id);
        }

        public int DeleteFishSeller(int id)
        {
            return CreateSegementAppEntityInstance.fishSegmentDomainService.DeleteFishSeller(id);
        }
    }
}
