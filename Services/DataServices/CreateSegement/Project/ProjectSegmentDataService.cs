using Core.DataService;
using Services.DataContext;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.CreateSegement.SPModels;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.CreateSegement.Project
{
    public class ProjectSegmentDataService : EntityContextDataService<CreateSegmentProject>
    {
        public ProjectSegmentDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }


        public bool ProcessSegmentProject(CreateSegmentProjectModel createSegmentProjectModel, int adminUserId)
        {


            if (createSegmentProjectModel != null && createSegmentProjectModel.isAddOperation == true)
            {
                CreateSegmentProject segmentProject = new CreateSegmentProject(); 
                segmentProject.ID = createSegmentProjectModel.createSegmentProject.ID;
                segmentProject.Name = createSegmentProjectModel.createSegmentProject.Name;

                if (createSegmentProjectModel.createSegmentProject.ImageUrl != null)
                {
                    segmentProject.ImageUrl = createSegmentProjectModel.createSegmentProject.ImageUrl;
                }
                else {
                    segmentProject.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentProject.AreaId = createSegmentProjectModel.createSegmentProject.AreaId;
                segmentProject.Land = createSegmentProjectModel.createSegmentProject.Land;
                segmentProject.Cost = createSegmentProjectModel.createSegmentProject.Cost;
                segmentProject.Time = createSegmentProjectModel.createSegmentProject.Time;
                segmentProject.IsDeleted = false;

                segmentProject.CreatedDate = DateTime.Now;
                segmentProject.CreatedId = adminUserId;
                DbContext.Set<CreateSegmentProject>().Add(segmentProject);
                DbContext.SaveChanges();

                return true;
            }

            if (createSegmentProjectModel != null && createSegmentProjectModel.isAddOperation == false)
            {
                CreateSegmentProject segmentProject = DbContext.Set<CreateSegmentProject>().Where(x => x.ID == createSegmentProjectModel.createSegmentProject.ID).FirstOrDefault();

                segmentProject.Name = createSegmentProjectModel.createSegmentProject.Name;

                if (createSegmentProjectModel.createSegmentProject.ImageUrl != null)
                {
                    segmentProject.ImageUrl = createSegmentProjectModel.createSegmentProject.ImageUrl;
                }
                else {
                    segmentProject.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                segmentProject.AreaId = createSegmentProjectModel.createSegmentProject.AreaId;
                segmentProject.Land = createSegmentProjectModel.createSegmentProject.Land;
                segmentProject.Cost = createSegmentProjectModel.createSegmentProject.Cost;
                segmentProject.Time = createSegmentProjectModel.createSegmentProject.Time;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public IList<SPCreateSegmentProjectList> GetSegmentProjectListBySearch(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<SPCreateSegmentProjectList> segmentFishList = DbContext.Database.SqlQuery<SPCreateSegmentProjectList>("up_GetSegmentProjectListBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return segmentFishList;
        }


        public CreateSegmentProject GetSegmentProjectDetailsByProjectId(int projectId)
        {
            CreateSegmentProject projectDetails = new CreateSegmentProject();
            projectDetails = DbContext.Set<CreateSegmentProject>().Where(x => x.ID == projectId).FirstOrDefault();
            return projectDetails;
        }

        public IList<CreateSegmentProject> GetAllProjectList()
        {
            List<CreateSegmentProject> segmentAreaList = DbContext.Database.SqlQuery<CreateSegmentProject>("SELECT * FROM PROJECT").ToList();

            return segmentAreaList;
        }

        public IList<CreateSegmentProject> GetProjectListByAreaId(int areaId)
        {
            List<CreateSegmentProject> projectList = DbContext.Database.SqlQuery<CreateSegmentProject>("select * from Project where AreaId = " + areaId + ";").ToList();

            return projectList;
        }




        public int DeleteProject(int id)
        {

            //int result = DbContext.Database.ExecuteSqlCommand("update [Project] set IsDeleted = 1 where id=" + id);
            int result = DbContext.Database.ExecuteSqlCommand("Delete from Project where id=" + id);

            return result;
        }

    }
}
