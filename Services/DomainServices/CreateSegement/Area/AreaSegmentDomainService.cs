using Core.DomainService;
using Services.DataServices.CreateSegement.Area;
using Services.Domain.CostReport;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.SPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.CreateSegement.Area
{
    public class AreaSegmentDomainService : DomainService<CreateSegmentArea, long>
    {
        private readonly AreaSegmentDataService _areaSegmentDataService;
        public AreaSegmentDomainService(AreaSegmentDataService areaSegmentDataService) : base(areaSegmentDataService)
        {
            _areaSegmentDataService = areaSegmentDataService;
        }

        #region "Get Segment Fish list"
        public IList<SPCreateSegmentAreaList> GetSegmentAreaListBySearch(string searchText, int pageNumber, int pageSize)
        {
            return _areaSegmentDataService.GetSegmentAreaListBySearch(searchText, pageNumber, pageSize);
        }
        #endregion

        #region "Process segment feed"
        public bool ProcessSegmentArea(CreateSegmentAreaModel createSegmentAreaModel, int adminUserId)
        {
            return _areaSegmentDataService.ProcessSegmentArea(createSegmentAreaModel, adminUserId);
        }
        #endregion

        #region "Get Segment Area Info"
        public CreateSegmentArea GetSegmentAreaDetailsByAreaId(int areaId)
        {
            return _areaSegmentDataService.GetSegmentAreaDetailsByAreaId(areaId);
        }

        #endregion

        public IList<CreateSegmentArea> GetAllAreaList()
        {
            return _areaSegmentDataService.GetAllAreaList();
        }
        public IList<CostExpandTable> GetAllExpandList()
        {
            return _areaSegmentDataService.GetAllExpandList();
        }

        public int DeleteArea(int id)
        {
            return _areaSegmentDataService.DeleteArea(id);
        }

    }
}
