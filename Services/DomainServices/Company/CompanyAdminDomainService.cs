using Core.DomainService;
using Services.AppServices;

using Services.DataServices.Company;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.Models.CompanyAdmin.EditorModel;
using Services.Domain.Company.SPModels;
using Services.Domain.Models.User.EditorModel;
using Services.Domain.Models.User.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.DomainServices.Company
{
 public   class CompanyAdminDomainService : DomainService<CompanyAdmin, long>
    {
        private readonly CompanyAdminDataService _companyAdminDataService;
        public CompanyAdminDomainService(CompanyAdminDataService companyAdminDataService) : base(companyAdminDataService)
        {
            _companyAdminDataService = companyAdminDataService;
        }

        #region "Company Admin Image Upload From  add/edit section"
        public string CompanyAdminImageUpload(HttpPostedFileBase file, Guid fileNameInGuid,long companyId, bool isEditMode)
        {
            //string actualFileName = file.FileName;
            //string fileName = fileNameInGuid + Path.GetExtension(file.FileName);
            //long size = file.ContentLength;
            //MemoryStream fileStream = new MemoryStream();

            //string imageTitle = Path.GetFileNameWithoutExtension(file.FileName);
            //string imagePath = AzureBlobFolderName.USER_LOGO_FOLDER_NAME + "/" + fileName;

            //if (isEditMode == true)
            //{
            //   _companyAdminDataService.UpdateCompanyAdminImage(companyId, actualFileName, imagePath);
            //}

            //file.InputStream.CopyTo(fileStream);
            ////=== Saving File In Azure ==============================
            //AzureStorageService.UploadFileInAzureStorageAsync(fileStream.ToArray(), AzureBlobFolderName.USER_LOGO_FOLDER_NAME, fileName);
            ////================================================

            //return imagePath;
            return "";
        }
        #endregion


        #region "Get Company Admin List In Super Admin"
        public IList<SMDLCompanyAdminList> GetCompanyAdminBySuperAdminAndBySearch(string searchText, int pageNumber, int pageSize)
        {
            return _companyAdminDataService.GetCompanyAdminBySuperAdminAndBySearch(searchText, pageNumber, pageSize);
        }
        #endregion

        #region "Process Company Admin"
        public bool ProcessCompanyAdminFromSuperAdmin(CompanyManagementModel companyManagementModel, long adminUserId)
        {
            return _companyAdminDataService.ProcessCompanyAdminFromSuperAdmin(companyManagementModel, adminUserId);
        }
        #endregion


        #region Delete Company Related Method
        public bool DeleteCompanyAdminInfoBySuperAdmin(long companyID)
        {
            return _companyAdminDataService.DeleteCompanyAdminInfoBySuperAdmin(companyID);
        }
        #endregion

        #region "Get Company Admin Info"
        public CompanyAdmin GetCompanyAdminInfoByCompanyID(long companyID)
        {
            return _companyAdminDataService.GetCompanyAdminInfoByCompanyID(companyID);
        }

        #endregion

    }
}
