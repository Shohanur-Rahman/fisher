using Kendo.Mvc.UI;
using Services.ApplicationEntity.Company;
using Services.ApplicationEntity.CreateSegement;
using Services.ApplicationServices.Company;
using Services.ApplicationServices.CreateSegement.Area;
using Services.ApplicationServices.CreateSegement.Project;
using Services.Domain;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.Models.User;
using Services.DomainServices.Company;
using Services.DomainServices.CreateSegement.Area;
using Services.DomainServices.CreateSegement.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers
{
    public class UserController : BaseController
    {

        #region "Declare Domain Service's objects Needed for Company User"

        private readonly CompanyUserDomainService _companyUserDomainService;
        protected AreaSegmentApplicationService _areaSegmentApplicationService;
        protected ProjectSegmentApplicationService _projectSegmentApplicationService;

        #endregion

        #region "Application Service object and Application Entity object For Company User "
        protected CreateSegementAppEntity _createSegementAppEntity;
        protected CompanyUserAppEntity companyUserAppEntityInstance;
        protected CompanyUserApplicationService companyUserApplicationService;

        #endregion

        #region "Constructor"
        public UserController(CompanyUserDomainService contextUser, AreaSegmentDomainService areaSegmentDomainService, ProjectSegmentDomainService projectSegmentDomainService)
        {

            _companyUserDomainService = contextUser;
            
            //Application Entity: Initialize and assign domain objects in Application Entity for company user
            companyUserAppEntityInstance = new CompanyUserAppEntity();

            companyUserAppEntityInstance.CompanyUserDomainService = _companyUserDomainService;

            //Application Service: Initialize Application Service object for company user Module
            companyUserApplicationService = new CompanyUserApplicationService(companyUserAppEntityInstance);
            _createSegementAppEntity = new CreateSegementAppEntity();
            _createSegementAppEntity.areaSegmentDomainService = areaSegmentDomainService;
            _createSegementAppEntity.projectSegmentDomainService = projectSegmentDomainService;
            _areaSegmentApplicationService = new AreaSegmentApplicationService(_createSegementAppEntity);
            _projectSegmentApplicationService = new ProjectSegmentApplicationService(_createSegementAppEntity);
        }
        #endregion

        public object CustomUrlHelper { get; private set; }

        // GET: User
        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.UserInformationMessage = message;

            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.UserDeleteInformationMessage = deleteMsg;
            

            return View("~/Views/User/Index.cshtml");
        }


        public ActionResult GetProjectListByAreaId(int areaID)
        {
            IList<CreateSegmentProject> projectList = _projectSegmentApplicationService.GetProjectListByAreaId(areaID);

            return Json(projectList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddUser()
        {
            CompanyUserModel operationResult = new CompanyUserModel();
            ViewBag.ListOfArea = new SelectList(_areaSegmentApplicationService.GetAllAreaList(), "ID", "Name");
            ViewBag.ListOfProject = new SelectList(_projectSegmentApplicationService.GetAllProjectList(), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(CompanyUserModel companyUserModel)
        {
            CompanyUserModel operationResult = new CompanyUserModel();

            return DataActionViewService(
                () =>
                {
                    if (companyUserModel != null)
                    {
                        companyUserModel.IsAddOperation = true;
                        companyUserModel.AreaId = 0;
                        operationResult = companyUserApplicationService.ProcessCompanyUserByCompanyAdminWithAllInfo( companyUserModel);
                    }

                },
                () =>
                {
                    return RedirectToAction("Index", "User");
                },
                () => View("~/User/Index.cshtml"));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(CompanyUserModel companyUserModel)
        {
            CompanyUserModel operationResult = new CompanyUserModel();

            return DataActionViewService(
                () =>
                {
                    if (companyUserModel != null)
                    {
                        companyUserModel.IsAddOperation = false;
                        operationResult = companyUserApplicationService.ProcessCompanyUserByCompanyAdminWithAllInfo(companyUserModel);
                    }

                },
                () =>
                {
                    return RedirectToAction("Index", "User");
                },
                () => View("~/User/Index.cshtml"));
        }


        public ActionResult DeleteUser(int id)
        {
            int result = companyUserApplicationService.DeleteUser(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserSearchList(string searchName) {

            int totalRows = 0;
            int areaId = LoggedInUserInfoFromCookie.UserAreaIdInCookie;
            int projectId = (int)LoggedInUserInfoFromCookie.UserProjectIdInCookie;
            IList<SPUserSearchListResult> userList = companyUserApplicationService.GetUserSearchList(searchName);
            totalRows = userList.Count;

            var result = new DataSourceResult()
            {
                Data = userList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int id)
        {
            CompanyUserModel userDetails = new CompanyUserModel();
            User userInfo = new User();
            return DataActionViewService(
               () =>
               {
                   ViewBag.ListOfArea = new SelectList(_areaSegmentApplicationService.GetAllAreaList(), "ID", "Name");
                   ViewBag.ListOfProject = new SelectList(_projectSegmentApplicationService.GetAllProjectList(), "ID", "Name");
                   userDetails = companyUserApplicationService.GetUserDetailsByUserId(id);
                   ViewBag.AreaId = userDetails.AreaId;
               },
               () =>
               {
                   if (userDetails != null)
                   {
                       return View("~/Views/User/Edit.cshtml", userDetails);
                   }

                   return View("~/Views/User/Edit.cshtml");
               },
               () => View("~/Views/User/Edit.cshtml"));
        }


        public string UploadUserImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/User");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            return "Uploads/User/" + saVingFileName;
        }


    }
}