using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models.Minnow;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Accounts
{
    public class MinnowReportDataService : EntityContextDataService<MinnowReportTable>
    {
        public MinnowReportDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }
    }
}
