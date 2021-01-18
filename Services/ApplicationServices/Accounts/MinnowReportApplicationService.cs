using Services.ApplicationEntity.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Accounts
{
    public class MinnowReportApplicationService
    {
        protected MinnowReportAppEntity _MinnowReportAppEntityInstance { get; set; }

        public MinnowReportApplicationService(MinnowReportAppEntity minnowReportAppEntity)
        {
            _MinnowReportAppEntityInstance = minnowReportAppEntity;
        }
    }
}
