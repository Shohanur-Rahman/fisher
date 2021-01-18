using Services.ApplicationEntity.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Company
{
   public class CompanyUserDashbordApplicationService
    {

        protected CompanyUserDashbordAppEntity userDashboardAppEntityInstance { get; set; }

        public CompanyUserDashbordApplicationService(CompanyUserDashbordAppEntity userDashbordAppEntity)
        {
            userDashboardAppEntityInstance = userDashbordAppEntity;
        }

     
    }
}
