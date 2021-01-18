using Services.ApplicationEntity.CreateSegement;
using Services.Domain.CostReport;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.CreateSegement.Area
{
   public  class AreaSegmentApplicationService
    {
        protected CreateSegementAppEntity CreateSegementAppEntityInstance { get; set; }

        public AreaSegmentApplicationService(CreateSegementAppEntity createSegementAppEntity)
        {
            CreateSegementAppEntityInstance = createSegementAppEntity;
        }

        #region "Get Fish List"
        public IList<SPCreateSegmentAreaList> GetSegmentAreaListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return CreateSegementAppEntityInstance.areaSegmentDomainService.GetSegmentAreaListBySearch(searchText, pageNumber, pageSize);
        }

        #endregion

        #region "Process Area Segment"
        public bool ProcessSegmentArea(CreateSegmentAreaModel createSegmentAreaModel, int adminUserId)
        {
            return CreateSegementAppEntityInstance.areaSegmentDomainService.ProcessSegmentArea(createSegmentAreaModel, adminUserId);
        }

        #endregion

        #region "Get Area Details"
        public CreateSegmentArea GetSegmentAreaDetailsByAreaId(int areaId)
        {
            CreateSegmentArea createSegmentArea = CreateSegementAppEntityInstance.areaSegmentDomainService.GetSegmentAreaDetailsByAreaId(areaId);
            if (createSegmentArea != null)
            {
                return createSegmentArea;

            }
            return null;
        }

        #endregion

        public IList<CreateSegmentArea> GetAllAreaList()
        {
            return CreateSegementAppEntityInstance.areaSegmentDomainService.GetAllAreaList();
        }
        public IList<CostExpandTable> GetAllExpandList()
        {
            return CreateSegementAppEntityInstance.areaSegmentDomainService.GetAllExpandList();
        }


        public int DeleteArea(int id)
        {
            return CreateSegementAppEntityInstance.areaSegmentDomainService.DeleteArea(id);
        }

    }
}
