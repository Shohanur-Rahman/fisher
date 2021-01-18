using Services.ApplicationEntity.Company;
using Services.Domain;
using Services.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Company
{

   public  class CompanyUserApplicationService
    {
        protected CompanyUserAppEntity companyUserAppEntityInstance { get; set; }


        public CompanyUserApplicationService(CompanyUserAppEntity companyUserAppEntity)
        {
            companyUserAppEntityInstance = companyUserAppEntity;
        }

        #region "Company User Create or edit From Company Admin"

        public CompanyUserModel ProcessCompanyUserByCompanyAdminWithAllInfo(CompanyUserModel companyUserModel)
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.ProcessCompanyUserByCompanyAdminWithAllInfo(companyUserModel);
        }

        public int DeleteUser(int id)
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.DeleteUser(id);
        }
        public CompanyUserModel GetUserDetailsByUserId(int Id)
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.GetUserDetailsByUserId(Id);
        }

        public IList<SPUserSearchListResult> GetUserSearchList(string searchName)
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.GetUserSearchList(searchName);
        }


        public IList<SPFishSellingChartReport> GetAdminDashBoardFishSellingReport()
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.GetAdminDashBoardFishSellingReport();
        }

        public SPAdminDashBoardOveriew GetAdminDashBoardOverview()
        {
            return companyUserAppEntityInstance.CompanyUserDomainService.GetAdminDashBoardOverview();
        }

        #endregion
    }
}
