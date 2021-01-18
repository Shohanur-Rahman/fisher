using Services.ApplicationEntity;
using Services.ApplicationEntity.Company;
using Services.AppServices;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.Models.CompanyAdmin.EditorModel;
using Services.Domain.Company.SPModels;
using Services.Domain.Models.User.EditorModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.ApplicationServices
{
    public  class CompanyAdminApplicationService
    {
        protected CompanyAdminAppEntity CompanyAdminAppEntityInstance { get; set; }

        public CompanyAdminApplicationService(CompanyAdminAppEntity companyAdminAppEntity)
        {
            CompanyAdminAppEntityInstance = companyAdminAppEntity;
        }

        #region "Get Company Admin List In Super Admin"
        public IList<SMDLCompanyAdminList> GetCompanyAdminBySuperAdminAndBySearch(string searchText, int pageNumber, int pageSize)
        {
            return CompanyAdminAppEntityInstance.CompanyAdminDomainService.GetCompanyAdminBySuperAdminAndBySearch(searchText, pageNumber, pageSize);
        }
        #endregion

        #region "Process Company Admin"
        public bool ProcessCompanyAdminFromSuperAdmin(CompanyManagementModel companyManagementModel, long adminUserId)
        {
            return CompanyAdminAppEntityInstance.CompanyAdminDomainService.ProcessCompanyAdminFromSuperAdmin(companyManagementModel,  adminUserId);
        }
        #endregion


        #region Delete Company Related Method
        public bool DeleteCompanyAdminInfoBySuperAdmin(long companyID)
        {
            return CompanyAdminAppEntityInstance.CompanyAdminDomainService.DeleteCompanyAdminInfoBySuperAdmin(companyID);
        }
        #endregion

        #region "Get Company Admin Info"
        public CompanyManagementModel GetCompanyAdminInfoByCompanyID(long companyID)
        {
           CompanyManagementModel companyManagementModel= null;
           CompanyAdmin companyAdmin= CompanyAdminAppEntityInstance.CompanyAdminDomainService.GetCompanyAdminInfoByCompanyID(companyID);
            if (companyAdmin != null)
            {
                companyManagementModel = new CompanyManagementModel();

                companyAdmin.Password = SimpleCryptService.Factory().Decrypt(companyAdmin.Password);
                companyAdmin.ConfirmPassword = SimpleCryptService.Factory().Decrypt(companyAdmin.ConfirmPassword);
                companyManagementModel.IsActive = companyAdmin.IsActive;
                companyManagementModel.CustomURL = companyAdmin.CustomURL;
                companyManagementModel.LicenseStartDate = companyAdmin.LicenseStartDate.ToString();
                companyManagementModel.LicenseEndDate = companyAdmin.LicenseEndDate.ToString();
             
                companyManagementModel.DataUploadForHazardAndTrade = companyAdmin.DataUploadForHazardAndTrade;
             



                companyManagementModel.CompanyAdmin = companyAdmin;
          

            }
            return companyManagementModel;
        }

        #endregion

  

    }
}
