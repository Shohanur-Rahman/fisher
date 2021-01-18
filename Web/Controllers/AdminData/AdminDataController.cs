using Kendo.Mvc.UI;
using Services.ApplicationEntity.Accounts;
using Services.ApplicationEntity.CreateSegement;
using Services.ApplicationServices.Accounts;
using Services.ApplicationServices.CreateSegement.Project;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.Models.Project;
using Services.DomainServices.Accounts;
using Services.DomainServices.CreateSegement.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;
namespace Web.Controllers
{
    public class AdminDataController : BaseController
    {
        // GET: AdminData

        private readonly FishSellingDomainServices _fishSellingDomainServices;

        protected FishSellingReportAppEntity _fishSellingReportAppEntity;
        protected FishSellingReportApplicationServices _fishSellingReportApplicationServices;
        protected ProjectSegmentApplicationService _projectSegmentApplicationService;
        protected CreateSegementAppEntity _createSegementAppEntity;
        public AdminDataController(FishSellingDomainServices fishSellingDomainServices, ProjectSegmentDomainService projectSegmentDomainService)
        {
            _fishSellingDomainServices = fishSellingDomainServices;
            _fishSellingReportAppEntity = new FishSellingReportAppEntity();
            _fishSellingReportAppEntity.fishSellingDomainServices = fishSellingDomainServices;
            _fishSellingReportApplicationServices = new FishSellingReportApplicationServices(_fishSellingReportAppEntity);

            _createSegementAppEntity = new CreateSegementAppEntity();
            _createSegementAppEntity.projectSegmentDomainService = projectSegmentDomainService;
            _projectSegmentApplicationService = new ProjectSegmentApplicationService(_createSegementAppEntity);

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FishSellReport()
        {
            return View("~/Views/AdminData/FishSellReport.cshtml");
        }

        public ActionResult FishSellerInfo() {
            ViewBag.SelectedFishMultiSelectList = null;
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/AdminData/FishSellerInfo.cshtml");
        }


        public ActionResult GetFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectId, string sellerId)
        {
            int totalRows = 0;

            IList<SPFishSellingSellerReportForAdminByParam> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial, projectId, sellerId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTotalFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectId, string sellerId)
        {
            SPFishSellingReportGetTotalForAdminRecord sellingReportList = _fishSellingReportApplicationServices.GetTotalFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial, projectId, sellerId);
            
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFishSellingReportForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectId)
        {
            int totalRows = 0;
            
            IList<SPFishSellingReportListForAdminBySearchParam> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingReportForAdminByParams(startDate, endDate, isPartial, areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFishSellingReportTotalForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectId)
        {


            SPFishSellingReportGetTotalForAdminRecord sellingReportList = _fishSellingReportApplicationServices.GetFishSellingReportTotalForAdminByParams(startDate, endDate, isPartial, areaId, projectId);
            
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllFishSellerNameList()
        {
            IList<CreateSegmentFishSeller> sellingReportList = _fishSellingReportApplicationServices.GetAllFishSellerNameList();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllProjectList()
        {
            IList<CreateSegmentProject> sellingReportList = _projectSegmentApplicationService.GetAllProjectList();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

    }
}