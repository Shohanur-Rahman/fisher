using Core.DomainService;
using Services.DataServices.CostReport;
using Services.Domain.CostReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.CostReport
{
    public class CostReportDomainService : DomainService<CostExpandTable, long>
    {
        private readonly CostReportDataService _costReportDataService;
        public CostReportDomainService(CostReportDataService costReportDataService) : base(costReportDataService)
        {
            _costReportDataService = costReportDataService;
        }

        public bool ProcessCostName(CostReportDomains cCostReportDomains, int adminUserId)
        {
            return _costReportDataService.ProcessCostName(cCostReportDomains, adminUserId);
        }

        public IList<CostExpandNamePorperty> GetCostExpandNameList(string searchText)
        {
            return _costReportDataService.GetCostExpandNameList(searchText);
        }

        public CostExpandTable GetCostExpandNameDetails(int id)
        {
            return _costReportDataService.GetCostExpandNameDetails(id);
        }

        public int DeleteCostExpandName(int id)
        {
            return _costReportDataService.DeleteCostExpandName(id);
        }


    }
}
