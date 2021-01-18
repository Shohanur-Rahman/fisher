using Core.DataService;
using Services.AppServices;
using Services.DataContext;
using Services.Domain;
using Services.Domain.Company;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.Models.CompanyAdmin.EditorModel;
using Services.Domain.Company.SPModels;
using Services.Domain.Models.User.EditorModel;
using Services.Helper;
using Services.RandomPasswordService;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Company
{
    public class CompanyAdminDataService : EntityContextDataService<CompanyAdmin>
    {
        public CompanyAdminDataService(AppDbContext dbContext) : base(dbContext)
        { 
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }


        #region "Get Company Admin List In Super Admin"
        public IList<SMDLCompanyAdminList> GetCompanyAdminBySuperAdminAndBySearch(string searchText, int pageNumber, int pageSize)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);
            SqlParameter prmPageNumber = new SqlParameter("@pageNumber", pageNumber);
            SqlParameter prmPageSize = new SqlParameter("@pageSize", pageSize);

            List<SMDLCompanyAdminList> companyAdminList = DbContext.Database.SqlQuery<SMDLCompanyAdminList>("GetCompanyAdminListBySuperAdminAndBySearch @searchText,@pageNumber,@pageSize", prmSearchText, prmPageNumber, prmPageSize).ToList();

            return companyAdminList;
        }
        #endregion

        #region "Process Company Admin From Super Admin"

        public bool ProcessCompanyAdminFromSuperAdmin(CompanyManagementModel companyManagementModel, long adminUserId)
        {


            if (companyManagementModel != null && companyManagementModel.isAddOperation == true)
            {
                CompanyAdmin companyAdmin = new CompanyAdmin();

                string encryptNewPass = SimpleCryptService.Factory().Encrypt(companyManagementModel.CompanyAdmin.Password);
                string encryptNewConfirmPass = SimpleCryptService.Factory().Encrypt(companyManagementModel.CompanyAdmin.ConfirmPassword);

                companyAdmin.CompanyName = companyManagementModel.CompanyAdmin.CompanyName;

                if (companyManagementModel.CompanyAdmin.CompanyLogoPath != null)
                {
                    companyAdmin.CompanyLogoPath = companyManagementModel.CompanyAdmin.CompanyLogoPath;
                }
                if (companyManagementModel.CompanyAdmin.CompanyLogoCaption != null)
                {
                    companyAdmin.CompanyLogoCaption = companyManagementModel.CompanyAdmin.CompanyLogoCaption;
                }
                companyAdmin.EmailAddress = companyManagementModel.CompanyAdmin.EmailAddress;

                companyAdmin.Password = encryptNewPass;
                companyAdmin.ConfirmPassword = encryptNewConfirmPass;
                companyAdmin.Phone1 = companyManagementModel.CompanyAdmin.Phone1;
                companyAdmin.Phone2 = companyManagementModel.CompanyAdmin.Phone2;
                companyAdmin.Address1 = companyManagementModel.CompanyAdmin.Address1;
                companyAdmin.Address2 = companyManagementModel.CompanyAdmin.Address2;
                companyAdmin.CompanyTown = companyManagementModel.CompanyAdmin.CompanyTown;
                companyAdmin.CompanyPostCode = companyManagementModel.CompanyAdmin.CompanyPostCode;
                companyAdmin.WebAddress = companyManagementModel.CompanyAdmin.WebAddress;
                companyAdmin.CompanyDetails = companyManagementModel.CompanyAdmin.CompanyDetails;
                companyAdmin.LicenseStartDate = companyManagementModel.CompanyAdmin.LicenseStartDate;
                companyAdmin.LicenseEndDate = companyManagementModel.CompanyAdmin.LicenseEndDate;
                companyAdmin.HowManyLicense = companyManagementModel.CompanyAdmin.HowManyLicense;          
                companyAdmin.IsActive = companyManagementModel.CompanyAdmin.IsActive;
                companyAdmin.CustomURL = companyManagementModel.CompanyAdmin.CustomURL;
                companyAdmin.DataUploadForHazardAndTrade = companyManagementModel.CompanyAdmin.DataUploadForHazardAndTrade;
                companyAdmin.CreatedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                companyAdmin.EditedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                companyAdmin.CreatedBy = adminUserId;
                companyAdmin.EditedBy = adminUserId;
                companyAdmin.CreatedByUserRole = Services.Domain.Models.Role.Enums.Role.SuperAdmin.ToString();
                companyAdmin.EditedByUserRole = Services.Domain.Models.Role.Enums.Role.SuperAdmin.ToString();
                companyAdmin.Role = Services.Domain.Models.Role.Enums.Role.CompanyAdmin.ToString();
                companyAdmin.CreatedDate = DateTime.Now;
                companyAdmin.EditedDate = DateTime.Now;
                DbContext.Set<CompanyAdmin>().Add(companyAdmin);
                DbContext.SaveChanges();
 
                return true;
            }

            if (companyManagementModel != null && companyManagementModel.isAddOperation == false)
            {
                CompanyAdmin companyAdmin = DbContext.Set<CompanyAdmin>().Where(x => x.CompanyID == companyManagementModel.CompanyAdmin.CompanyID).FirstOrDefault();
                if (companyAdmin != null)
                {
                    string encryptNewPass = SimpleCryptService.Factory().Encrypt(companyManagementModel.CompanyAdmin.Password);
                    string encryptNewConfirmPass = SimpleCryptService.Factory().Encrypt(companyManagementModel.CompanyAdmin.ConfirmPassword);

                    companyAdmin.CompanyName = companyManagementModel.CompanyAdmin.CompanyName;

                   
                    companyAdmin.EmailAddress = companyManagementModel.CompanyAdmin.EmailAddress;

                    companyAdmin.Password = encryptNewPass;
                    companyAdmin.ConfirmPassword = encryptNewConfirmPass;
                    companyAdmin.Phone1 = companyManagementModel.CompanyAdmin.Phone1;
                    companyAdmin.Phone2 = companyManagementModel.CompanyAdmin.Phone2;
                    companyAdmin.Address1 = companyManagementModel.CompanyAdmin.Address1;
                    companyAdmin.Address2 = companyManagementModel.CompanyAdmin.Address2;
                    companyAdmin.CompanyTown = companyManagementModel.CompanyAdmin.CompanyTown;
                    companyAdmin.CompanyPostCode = companyManagementModel.CompanyAdmin.CompanyPostCode;
                    companyAdmin.WebAddress = companyManagementModel.CompanyAdmin.WebAddress;
                    companyAdmin.CompanyDetails = companyManagementModel.CompanyAdmin.CompanyDetails;
                    companyAdmin.LicenseStartDate = companyManagementModel.CompanyAdmin.LicenseStartDate;
                    companyAdmin.LicenseEndDate = companyManagementModel.CompanyAdmin.LicenseEndDate;
                    companyAdmin.HowManyLicense = companyManagementModel.CompanyAdmin.HowManyLicense;
           
                    companyAdmin.IsActive = companyManagementModel.CompanyAdmin.IsActive;
                    companyAdmin.CustomURL = companyManagementModel.CompanyAdmin.CustomURL;
                    companyAdmin.DataUploadForHazardAndTrade = companyManagementModel.CompanyAdmin.DataUploadForHazardAndTrade;
                    companyAdmin.EditedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                    companyAdmin.EditedBy = adminUserId;
                    companyAdmin.EditedByUserRole = Services.Domain.Models.Role.Enums.Role.SuperAdmin.ToString();
                    companyAdmin.EditedDate = DateTime.Now;

                    DbContext.SaveChanges();
                    return true;

                }
            }
            return false;

        }

        #endregion

 
        #region Delete Company Related Method
        public bool DeleteCompanyAdminInfoBySuperAdmin(long companyID)
        {
            SqlParameter prmCompanyId = new SqlParameter("@companyID", companyID);
            int effectedRow = DbContext.Database.ExecuteSqlCommand("Company_DeleteCompanyAdmin_By_Super_Admin @companyID", prmCompanyId);

            return true;
        }
        #endregion

        #region "Get Company Admin Info"
        public CompanyAdmin GetCompanyAdminInfoByCompanyID(long companyID)
        {
            return DbContext.Set<CompanyAdmin>().Where(x => x.CompanyID == companyID).FirstOrDefault();
        }

        #endregion


    }

}
