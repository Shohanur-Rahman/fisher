using Core.DomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DataServices.FarmExpend;
using Services.Domain.FarmExpend;

namespace Services.DomainServices.FarmExpend
{
   public class FarmExpendDomainService : DomainService<FarmExpandTableDomain, long>
    {
        private readonly FarmExpendDataService _farmExpendDataService;
        public FarmExpendDomainService(FarmExpendDataService farmExpendDataService) : base(farmExpendDataService)
        {
            _farmExpendDataService = farmExpendDataService;
        }
        public bool ProcessFarmExpendReport(FarmExpandTableDomain farmExpendReport, int adminUserId)
        {
            return _farmExpendDataService.ProcessFarmExpandReport(farmExpendReport, adminUserId);
        }

        public FarmExpandTableDomain GetFarmExpendDetailsByExpendID(int id)
        {
            return _farmExpendDataService.GetFarmExpendDetailsByExpendID(id);
        }
    }
}
