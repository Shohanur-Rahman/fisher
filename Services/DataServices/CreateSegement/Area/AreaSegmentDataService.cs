using Core.DataService;
using Services.DataContext;
using Services.Domain.CostReport;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.SPModels;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.CreateSegement.Area
{
    public class AreaSegmentDataService : EntityContextDataService<CreateSegmentArea>
    {
        public AreaSegmentDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        #region "Get Segment Area List"
        public IList<SPCreateSegmentAreaList> GetSegmentAreaListBySearch(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<SPCreateSegmentAreaList> segmentAreaList = DbContext.Database.SqlQuery<SPCreateSegmentAreaList>("up_GetSegmentAreaListBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return segmentAreaList;
        }
        #endregion


        #region "Process Segment Feed"

        public bool ProcessSegmentArea(CreateSegmentAreaModel createSegmentAreaModel, int adminUserId)
        {
             

            if (createSegmentAreaModel != null && createSegmentAreaModel.isAddOperation == true)
            {
                CreateSegmentArea segmentArea = new CreateSegmentArea();
                segmentArea.ID = createSegmentAreaModel.CreateSegmentArea.ID;
                segmentArea.Name = createSegmentAreaModel.CreateSegmentArea.Name;

                if (createSegmentAreaModel.CreateSegmentArea.ImageUrl != null)
                {
                    segmentArea.ImageUrl = createSegmentAreaModel.CreateSegmentArea.ImageUrl;
                }
                else {
                    segmentArea.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentArea.Union_Name = createSegmentAreaModel.CreateSegmentArea.Union_Name;
                segmentArea.WardNumber = createSegmentAreaModel.CreateSegmentArea.WardNumber;
                segmentArea.IsDeleted = false;

                segmentArea.CreatedDate = DateTime.Now;
                segmentArea.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentArea>().Add(segmentArea);
                DbContext.SaveChanges();

                return true;
            }

            if (createSegmentAreaModel != null && createSegmentAreaModel.isAddOperation == false)
            {
                CreateSegmentArea segmentArea = DbContext.Set<CreateSegmentArea>().Where(x => x.ID == createSegmentAreaModel.CreateSegmentArea.ID).FirstOrDefault();

                segmentArea.Name = createSegmentAreaModel.CreateSegmentArea.Name;

                if (createSegmentAreaModel.CreateSegmentArea.ImageUrl != null)
                {
                    segmentArea.ImageUrl = createSegmentAreaModel.CreateSegmentArea.ImageUrl;
                }
                else {
                    segmentArea.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentArea.Union_Name = createSegmentAreaModel.CreateSegmentArea.Union_Name;
                segmentArea.WardNumber = createSegmentAreaModel.CreateSegmentArea.WardNumber;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }

        #endregion



        #region "Get Segment Fish Info"
        public CreateSegmentArea GetSegmentAreaDetailsByAreaId(int areaId)
        {
            CreateSegmentArea areaDetails = new CreateSegmentArea();
            areaDetails = DbContext.Set<CreateSegmentArea>().Where(x => x.ID == areaId).FirstOrDefault();
            return areaDetails;
        }

        #endregion



        public IList<CreateSegmentArea> GetAllAreaList()
        {
            List<CreateSegmentArea> segmentAreaList = DbContext.Database.SqlQuery<CreateSegmentArea>("SELECT * FROM AREA").ToList();

            return segmentAreaList;
        }

        public IList<CostExpandTable> GetAllExpandList()
        {
            List<CostExpandTable> expendList = DbContext.Database.SqlQuery<CostExpandTable>("SELECT * FROM EXPEND").ToList();

            return expendList;
        }


        public int DeleteArea(int id)
        {

            //int result = DbContext.Database.ExecuteSqlCommand("update [Area] set IsDeleted = 1  where id=" + id);
            int result = DbContext.Database.ExecuteSqlCommand("delete from AREA  where id=" + id);

            return result;
        }


    }
}
