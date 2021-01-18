using Services.ApplicationEntity.CreateSegement;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.CreateSegement.Project
{
    public class ProjectSegmentApplicationService
    {
        protected CreateSegementAppEntity CreateSegementAppEntityInstance { get; set; }

        public ProjectSegmentApplicationService(CreateSegementAppEntity createSegementAppEntity)
        {
            CreateSegementAppEntityInstance = createSegementAppEntity;
        }

        #region "Process Area Segment"
        public bool ProcessSegmentProject(CreateSegmentProjectModel createSegmentProjectModel, int adminUserId)
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.ProcessSegmentProject(createSegmentProjectModel, adminUserId);
        }
        #endregion

        public IList<SPCreateSegmentProjectList> GetSegmentProjectListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.GetSegmentProjectListBySearch(searchText, pageNumber, pageSize);
        }

        public IList<CreateSegmentProject> GetSegmentProjectNameList()
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.GetAllProjectList();
        }


        public CreateSegmentProject GetSegmentProjectDetailsByProjectId(int projectId)
        {
            CreateSegmentProject createSegmentProject = CreateSegementAppEntityInstance.projectSegmentDomainService.GetSegmentProjectDetailsByProjectId(projectId);
            if (createSegmentProject != null)
            {
                return createSegmentProject;

            }
            return null;
        }
        public IList<CreateSegmentProject> GetAllProjectList()
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.GetAllProjectList();
        }

        public IList<CreateSegmentProject> GetProjectListByAreaId(int areaId)
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.GetProjectListByAreaId(areaId);
        }


        public int DeleteProject(int id)
        {
            return CreateSegementAppEntityInstance.projectSegmentDomainService.DeleteProject(id);
        }

    }
}
