using Core.DataService;
using Services.DataContext;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.SPModels;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.CreateSegement.Fish
{
    
     public class FishSegmentDataService : EntityContextDataService<CreateSegmentFish> 
    {
        public FishSegmentDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }


        // All region for Segment Fish

        #region"Segment Fish ============================================="



        #region "Get Segment Fish List"
        public IList<CreateSegmentFishList> GetSegmentFishListBySearch(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<CreateSegmentFishList> segmentFishList = DbContext.Database.SqlQuery<CreateSegmentFishList>("up_GetSegmentFishListBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return segmentFishList;
        }


        public IList<SPSegmentFishSellerList> GetSegmentFishSellerListBySearch(string searchText)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);

            List<SPSegmentFishSellerList> fishSellerList = DbContext.Database.SqlQuery<SPSegmentFishSellerList>("up_GetFishSellerSearchResults @searchText", prmSearchText).ToList();

            return fishSellerList;
        }

        #endregion


        public IList<SPCreateSegmentFeedCategoryList> GetSegmentFeedCategoryList(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<SPCreateSegmentFeedCategoryList> feedCategoryList = DbContext.Database.SqlQuery<SPCreateSegmentFeedCategoryList>("up_GetFeedCategorySearchResult @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return feedCategoryList;
        }


        #region "Process Segment Fish"

        public bool ProcessSegmentFish(CreateSegmentFishModel createSegmentFishModel, int adminUserId)
        {


            if (createSegmentFishModel != null && createSegmentFishModel.isAddOperation == true)
            {
                CreateSegmentFish segmentFish = new CreateSegmentFish();
                segmentFish.ID = createSegmentFishModel.CreateSegmentFish.ID;
                segmentFish.Name = createSegmentFishModel.CreateSegmentFish.Name;

                if (createSegmentFishModel.CreateSegmentFish.ImageUrl != null)
                {
                    segmentFish.ImageUrl = createSegmentFishModel.CreateSegmentFish.ImageUrl;
                }
                else {
                    segmentFish.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }
                 
                segmentFish.Description = createSegmentFishModel.CreateSegmentFish.Description;
                segmentFish.IsDeleted = false;
                segmentFish.CreatedDate = DateTime.Now;
                segmentFish.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentFish>().Add(segmentFish);
                DbContext.SaveChanges();

                return true;
            }

            if (createSegmentFishModel != null && createSegmentFishModel.isAddOperation == false)
            {
                CreateSegmentFish segmentFish = DbContext.Set<CreateSegmentFish>().Where(x => x.ID == createSegmentFishModel.CreateSegmentFish.ID).FirstOrDefault();

                segmentFish.Name = createSegmentFishModel.CreateSegmentFish.Name;

                if (createSegmentFishModel.CreateSegmentFish.ImageUrl != null)
                {
                    segmentFish.ImageUrl = createSegmentFishModel.CreateSegmentFish.ImageUrl;
                }
                else {
                    segmentFish.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentFish.Description = createSegmentFishModel.CreateSegmentFish.Description;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public bool ProcessSegmentFishSeller(CreateSegmentFishSellerModel createSegmentFishModel, int adminUserId)
        {


            if (createSegmentFishModel != null && createSegmentFishModel.isAddOperation == true)
            {
                CreateSegmentFishSeller segmentFish = new CreateSegmentFishSeller();
                segmentFish.ID = createSegmentFishModel.CreateSegmentFishSeller.ID;
                segmentFish.Name = createSegmentFishModel.CreateSegmentFishSeller.Name;

                if (createSegmentFishModel.CreateSegmentFishSeller.ImageUrl != null)
                {
                    segmentFish.ImageUrl = createSegmentFishModel.CreateSegmentFishSeller.ImageUrl;
                }
                else
                {
                    segmentFish.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentFish.Age = createSegmentFishModel.CreateSegmentFishSeller.Age;
                segmentFish.Description = createSegmentFishModel.CreateSegmentFishSeller.Description;
                segmentFish.IsDeleted = false;

                segmentFish.CreatedDate = DateTime.Now;
                segmentFish.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentFishSeller>().Add(segmentFish);
                DbContext.SaveChanges();

                return true;
            }

            if (createSegmentFishModel != null && createSegmentFishModel.isAddOperation == false)
            {
                CreateSegmentFishSeller segmentFish = DbContext.Set<CreateSegmentFishSeller>().Where(x => x.ID == createSegmentFishModel.CreateSegmentFishSeller.ID).FirstOrDefault();

                segmentFish.Name = createSegmentFishModel.CreateSegmentFishSeller.Name;

                if (createSegmentFishModel.CreateSegmentFishSeller.ImageUrl != null)
                {
                    segmentFish.ImageUrl = createSegmentFishModel.CreateSegmentFishSeller.ImageUrl;
                }
                else
                {
                    segmentFish.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentFish.Age = createSegmentFishModel.CreateSegmentFishSeller.Age;
                segmentFish.Description = createSegmentFishModel.CreateSegmentFishSeller.Description;
                DbContext.SaveChanges();

                return true;
            }

            return false;

        }

        #endregion

        #region "Get Segment Fish Info"
        public CreateSegmentFish GetSegmentFishDetailsByFishId(int fishID)
        {
            CreateSegmentFish fishDetails = new CreateSegmentFish();
            fishDetails = DbContext.Set<CreateSegmentFish>().Where(x => x.ID == fishID).FirstOrDefault();
            return fishDetails;
        }

        public CreateSegmentFishSeller GetSegmentFishSellerDetailsByFishId(int fishID)
        {
            CreateSegmentFishSeller fishDetails = new CreateSegmentFishSeller();
            fishDetails = DbContext.Set<CreateSegmentFishSeller>().Where(x => x.ID == fishID).FirstOrDefault();
            return fishDetails;
        }


        #endregion



        #endregion

        public int DeleteFish(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("Update [Fish] set IsDeleted = 1 where id=" + id);

            return result;
        }

        public int DeleteFishSeller(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("Update [FishSeller] set [IsDeleted] = 1  where id=" + id);

            return result;
        }

    }



}

