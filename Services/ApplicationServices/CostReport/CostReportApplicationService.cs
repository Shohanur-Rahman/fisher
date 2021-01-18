using Services.ApplicationEntity.CostReport;
using Services.Domain.CostReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.CostReport
{
    public class CostReportApplicationService
    {
        protected CostReportAppEntity _costReportAppEntityInstance { get; set; }
        public CostReportApplicationService(CostReportAppEntity costReportAppEntity)
        {
            _costReportAppEntityInstance = costReportAppEntity;
        }

        public bool ProcessCostName(CostReportDomains cCostReportDomains, int adminUserId)
        {
            return _costReportAppEntityInstance.costReportDomainService.ProcessCostName(cCostReportDomains, adminUserId);
        }
        public IList<CostExpandNamePorperty> GetCostExpandNameList(string searchText)
        {
            return _costReportAppEntityInstance.costReportDomainService.GetCostExpandNameList(searchText);
        }

        public CostExpandTable GetCostExpandNameDetails(int id)
        {
            return _costReportAppEntityInstance.costReportDomainService.GetCostExpandNameDetails(id);
        }

        public int DeleteCostExpandName(int id)
        {
            return _costReportAppEntityInstance.costReportDomainService.DeleteCostExpandName(id);
        }

    }

    
}
