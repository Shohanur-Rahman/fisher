using Core.DomainService;
using Services.DataServices.Company;
using Services.Domain;
using Services.Domain.Company.Models.CompanyAdmin.EditorModel;
using Services.Domain.Company.SPModels;
using Services.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Services.DomainServices.Company
{
    public class CompanyUserDomainService : DomainService<User, long>
    {
        private readonly CompanyUserDataService _companyUserDataService;
        public static Dictionary<string, int> dLoginFailed = new Dictionary<string, int>();
        public CompanyUserDomainService(CompanyUserDataService companyUserDataService) : base(companyUserDataService)
        {
            _companyUserDataService = companyUserDataService;
        }

        #region "Company User Create or edit From Company Admin"

        public CompanyUserModel ProcessCompanyUserByCompanyAdminWithAllInfo( CompanyUserModel companyUserModel)
        {
            return _companyUserDataService.ProcessCompanyUserByCompanyAdminWithAllInfo( companyUserModel);
        }
        public int DeleteUser(int id)
        {
            return _companyUserDataService.DeleteUser(id);
        }
        public CompanyUserModel GetUserDetailsByUserId(int Id)
        {
            return _companyUserDataService.GetUserDetailsByUserId(Id);
        }

        public IList<SPUserSearchListResult> GetUserSearchList(string searchName)
        {
            return _companyUserDataService.GetUserSearchList(searchName);
        }

        public IList<SPFishSellingChartReport> GetAdminDashBoardFishSellingReport()
        {
            return _companyUserDataService.GetAdminDashBoardFishSellingReport();
        }

        public SPAdminDashBoardOveriew GetAdminDashBoardOverview()
        {
            return _companyUserDataService.GetAdminDashBoardOverview();
        }
        #endregion
    }
}
