using Core.DomainService;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.Minnow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.Accounts
{
    public class MinnowReportDomainServices : DomainService<MinnowReportTable, long>
    {
        private readonly MinnowReportDataService _minnowReportDataService;
        public MinnowReportDomainServices(MinnowReportDataService minnowReportDataService) : base(minnowReportDataService)
        {
            _minnowReportDataService = minnowReportDataService;
        }
    }
}
