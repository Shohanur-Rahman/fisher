using Core.DomainService;
using Services.DataServices.CreateSegement.Project;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.CreateSegement.Project
{
    public class ProjectSegmentDomainService : DomainService<CreateSegmentProject, long>
    {
        private readonly ProjectSegmentDataService _projectSegmentDataService;
        public ProjectSegmentDomainService(ProjectSegmentDataService projectSegmentDataService) : base(projectSegmentDataService)
        {
            _projectSegmentDataService = projectSegmentDataService;
        }

        public bool ProcessSegmentProject(CreateSegmentProjectModel createSegmentProjectModel, int adminUserId)
        {
            return _projectSegmentDataService.ProcessSegmentProject(createSegmentProjectModel, adminUserId);
        }

        public IList<SPCreateSegmentProjectList> GetSegmentProjectListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return _projectSegmentDataService.GetSegmentProjectListBySearch(searchText, pageNumber, pageSize);
        }
        
        public CreateSegmentProject GetSegmentProjectDetailsByProjectId(int projectId)
        {
            return _projectSegmentDataService.GetSegmentProjectDetailsByProjectId(projectId);
        }


        public int DeleteProject(int id)
        {
            return _projectSegmentDataService.DeleteProject(id);
        }
        public IList<CreateSegmentProject> GetAllProjectList()
        {
            return _projectSegmentDataService.GetAllProjectList();
        }

        public IList<CreateSegmentProject> GetProjectListByAreaId(int areaId)
        {
            return _projectSegmentDataService.GetProjectListByAreaId(areaId);
        }

    }
}
