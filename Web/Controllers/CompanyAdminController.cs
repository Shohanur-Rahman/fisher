using Kendo.Mvc.UI;
using Services.ApplicationEntity;
using Services.ApplicationEntity.Company;
using Services.ApplicationServices;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.Models.CompanyAdmin.EditorModel;
using Services.Domain.Company.SPModels;
using Services.DomainServices.Company;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers
{

    [Authorize]
    public class CompanyAdminController : BaseController
    {
        #region "Declare Domain Service's objects Needed for Compnay Admin"

        private readonly CompanyAdminDomainService _companyAdminDomainService;

        #endregion

        #region "Application Service object and Application Entity object For Company admin"

        protected CompanyAdminAppEntity _companyAdminAppEntity;
        protected CompanyAdminApplicationService _companyAdminApplicationService;

        #endregion

        #region "Constructor"
        public CompanyAdminController(CompanyAdminDomainService companyAdminDomainService)
        {

            //Assign related domain services for Project Module
            _companyAdminDomainService = companyAdminDomainService;

            //Application Entity: Initialize and assign domain objects in Application Entity 
            _companyAdminAppEntity = new CompanyAdminAppEntity();
            _companyAdminAppEntity.CompanyAdminDomainService = companyAdminDomainService;
            //Application Service: Initialize Application Service object for Project Module
            _companyAdminApplicationService = new CompanyAdminApplicationService(_companyAdminAppEntity);
        }
        #endregion

        #region "HTML String For User Image"

        string companyAdminImageHtmlString = "<img src='###ImageURL###' id='divUplodedUserImg' class='img-responsive dummy_divUplodedUserImg' alt='User Logo' title='###ImageCaption###'>";

        #endregion

        #region Page load Related Method

        public ActionResult Test()
        {
            return View();
        }


        // GET: CompanyAdmin
        public ActionResult Index()
        {

            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }
        #endregion


        #region "Action For Company Admin Grid Related Data Load"

        public ActionResult GetCompanyAdminDataList([DataSourceRequest] DataSourceRequest request, string searchText)
        {
            int totalRows = 0;

            IList<SMDLCompanyAdminList> userList = _companyAdminApplicationService.GetCompanyAdminBySuperAdminAndBySearch(searchText, request.Page, request.PageSize);

            totalRows = (userList != null && userList.Count > 0) ? userList[0].TotalRows : 0;

            var result = new DataSourceResult()
            {
                Data = userList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Create Company Admin Related Method
        public ActionResult CreateCompanyAdmin()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompanyAdmin(CompanyManagementModel companyManagementModel)
        {
            bool isAddSuccess=false;
            long adminUserId = LoggedInUserInfoFromCookie.UserIdInCookie;
      
            return DataActionViewService(
                () =>
                {
                    if (companyManagementModel != null)
                    {

                        companyManagementModel.CompanyAdmin.LicenseStartDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseStartDate);
                        companyManagementModel.CompanyAdmin.LicenseEndDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseEndDate);

                        companyManagementModel.CompanyAdmin.IsActive = companyManagementModel.IsActive;
                        companyManagementModel.CompanyAdmin.CustomURL= companyManagementModel.CustomURL;
                        companyManagementModel.CompanyAdmin.DataUploadForHazardAndTrade = companyManagementModel.DataUploadForHazardAndTrade;
                        
                        companyManagementModel.isAddOperation = true;
                        isAddSuccess = _companyAdminApplicationService.ProcessCompanyAdminFromSuperAdmin(companyManagementModel,  adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {             
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, NotificationMessageContainer.COMPANY_ADMIN_ADDED_SUCCESS);
                        return RedirectToAction("Index", "CompanyAdmin");
                    }

                    return RedirectToAction("Index", "CompanyAdmin");
                },
                () => View());
       

        }

        public ActionResult ProcessCompanyAdmin(CompanyManagementModel companyManagementModel)
        {
            bool isAddSuccess = false;
            long adminUserId = LoggedInUserInfoFromCookie.UserIdInCookie;

            return DataActionViewService(
                () =>
                {
                    if (companyManagementModel != null)
                    {

                        companyManagementModel.CompanyAdmin.LicenseStartDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseStartDate);
                        companyManagementModel.CompanyAdmin.LicenseEndDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseEndDate);

                        companyManagementModel.CompanyAdmin.IsActive = companyManagementModel.IsActive;
                        companyManagementModel.CompanyAdmin.CustomURL = companyManagementModel.CustomURL;
                        companyManagementModel.CompanyAdmin.DataUploadForHazardAndTrade = companyManagementModel.DataUploadForHazardAndTrade;

                        companyManagementModel.isAddOperation = true;
                        isAddSuccess = _companyAdminApplicationService.ProcessCompanyAdminFromSuperAdmin(companyManagementModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, NotificationMessageContainer.COMPANY_ADMIN_ADDED_SUCCESS);
                        return Json(new { success = true  }, JsonRequestBehavior.AllowGet);
                    }

                    return RedirectToAction("Index", "CompanyAdmin");
                },
                () => View("~/Views/SuperAdmin/CompanyAdmin/Index.cshtml"));


        }
        #endregion

        #region Edit Company Admin Related Method

        //Get Method
        public ActionResult EditCompanyAdmin(long id)
        {
            long companyID = id;

            CompanyManagementModel companyManagementModel = null;
            long adminId = LoggedInUserInfoFromCookie.UserIdInCookie;
            string userRole = LoggedInUserInfoFromCookie.UserRoleInCookie;

            return DataActionViewService(
               () =>
               {
                   companyManagementModel = _companyAdminApplicationService.GetCompanyAdminInfoByCompanyID(companyID);
               },
               () =>
               {
                   if (companyManagementModel != null)
                   {       
                       return View(companyManagementModel);
                   }

                   return View();
               },
               () => View("~/Views/SuperAdmin/CompanyAdmin/Index.cshtml"));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompanyAdmin(CompanyManagementModel companyManagementModel)
        {
            bool isAddSuccess = false;
            long adminUserId = LoggedInUserInfoFromCookie.UserIdInCookie;

            return DataActionViewService(
                () =>
                {
                    if (companyManagementModel != null)
                    {

                        companyManagementModel.CompanyAdmin.LicenseStartDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseStartDate);
                        companyManagementModel.CompanyAdmin.LicenseEndDate = DateTimeHelpers.ConvertUKDateStringToDateTimeObject(companyManagementModel.LicenseEndDate);

                        companyManagementModel.CompanyAdmin.IsActive = companyManagementModel.IsActive;
                        companyManagementModel.CompanyAdmin.CustomURL = companyManagementModel.CustomURL;
                        companyManagementModel.CompanyAdmin.DataUploadForHazardAndTrade = companyManagementModel.DataUploadForHazardAndTrade;
                        companyManagementModel.isAddOperation = false;
                        isAddSuccess = _companyAdminApplicationService.ProcessCompanyAdminFromSuperAdmin(companyManagementModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, NotificationMessageContainer.COMPANY_ADMIN_EDIT_SUCCESS);
                        return RedirectToAction("Index", "CompanyAdmin");
                    }

                    return RedirectToAction("Index", "CompanyAdmin");
                },
                () => View());


        }



        #endregion

        #region Delete Company Related Method

        public ActionResult DeleteCompanyAdmin(long id)
        {
            long companyID = id;

            
            bool isDeleteSuccess = false;
            string userRole = LoggedInUserInfoFromCookie.UserRoleInCookie;

            return DataActionViewService(
                () =>
                {
                    if (userRole == Services.Domain.Models.Role.Enums.Role.SuperAdmin.ToString())
                    {
                        isDeleteSuccess = _companyAdminApplicationService.DeleteCompanyAdminInfoBySuperAdmin(companyID);
                    }

                },
                () =>
                {
                    if (isDeleteSuccess == true)
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, NotificationMessageContainer.COMPANY_ADMIN_DELETE_SUCCESS);
                        return RedirectToAction("Index", "CompanyAdmin");
                    }
                    return View("~/Views/SuperAdmin/CompanyAdmin/Index.cshtml");

                },
                () => View("~/Views/SuperAdmin/CompanyAdmin/Index.cshtml"));
        }
        #endregion


        #region "Company Admin Image Upload From  add/edit section"
        public ActionResult CreateCompanyAdminImageUpload(List<HttpPostedFileBase> files)
        {
            Guid fileNameInGuid;

            string imagePath = "";
            string actualFileName = "";
            bool isEditMode = false;

            return DataActionViewService(
                () =>
                {
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            fileNameInGuid = Guid.NewGuid();
                            actualFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            imagePath = _companyAdminDomainService.CompanyAdminImageUpload(file, fileNameInGuid,0, isEditMode);

                            break;
                        }
                    }
                },
                () =>
                {
                    if (imagePath != null || imagePath != "")
                    {
                        string imageURL = "";
                        string imageCaption = actualFileName;
                        companyAdminImageHtmlString = companyAdminImageHtmlString.Replace("###ImageURL###", imageURL);
                        companyAdminImageHtmlString = companyAdminImageHtmlString.Replace("###ImageCaption###", imageCaption);

                        return Json(new { DynamicHtml = companyAdminImageHtmlString, userUploadedImagePath = imagePath, userUploadedImageCaption = imageCaption }, JsonRequestBehavior.AllowGet);

                    }
                    return View(nameof(Index));

                },
                () => View(nameof(Index)));

        }

        public ActionResult EditCompanyAdminImageUpload(List<HttpPostedFileBase> files, long companyId)
        {
            Guid fileNameInGuid;

            string imagePath = "";
            string actualFileName = "";
            bool isEditMode = true;

            return DataActionViewService(
                () =>
                {
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            fileNameInGuid = Guid.NewGuid();
                            actualFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            imagePath = _companyAdminDomainService.CompanyAdminImageUpload(file, fileNameInGuid, companyId, isEditMode);

                            break;
                        }
                    }
                },
                () =>
                {
                    if (imagePath != null || imagePath != "")
                    {
                        string imageURL = "";
                        string imageCaption = actualFileName;
                        companyAdminImageHtmlString = companyAdminImageHtmlString.Replace("###ImageURL###", imageURL);
                        companyAdminImageHtmlString = companyAdminImageHtmlString.Replace("###ImageCaption###", imageCaption);

                        return Json(new { DynamicHtml = companyAdminImageHtmlString, userUploadedImagePath = imagePath, userUploadedImageCaption = imageCaption }, JsonRequestBehavior.AllowGet);

                    }
                    return View(nameof(Index));

                },
                () => View(nameof(Index)));

        }




        #endregion
    }
}