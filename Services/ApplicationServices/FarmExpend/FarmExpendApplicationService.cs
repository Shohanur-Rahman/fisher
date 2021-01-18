using Services.ApplicationEntity.FarmExpend;
using Services.Domain.FarmExpend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.FarmExpend
{
   public class FarmExpendApplicationService
    {
        protected FarmExpandAppEntity _farmExpandAppEntity { get; set; }

        public FarmExpendApplicationService(FarmExpandAppEntity farmExpandAppEntity)
        {
            _farmExpandAppEntity = farmExpandAppEntity;
        }
        public bool ProcessFarmExpendReport(FarmExpandTableDomain farmExpendReport, int adminUserId)
        {
            return _farmExpandAppEntity.farmExpendDomainService.ProcessFarmExpendReport(farmExpendReport, adminUserId);
        }
        public FarmExpandTableDomain GetFarmExpendDetailsByExpendID(int id)
        {
            FarmExpandTableDomain sellingReport = _farmExpandAppEntity.farmExpendDomainService.GetFarmExpendDetailsByExpendID(id);
            if (sellingReport != null)
            {
                return sellingReport;

            }
            return null;
        }

    }
}
