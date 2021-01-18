using Kendo.Mvc.UI;
using Services.ApplicationEntity.Accounts;
using Services.ApplicationServices.Accounts;
using Services.Domain.Accounts.Models.FarmRents;
using Services.Domain.CreateSegement.Models.Project;
using Services.DomainServices.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers
{
    public class FarmRentalsController : BaseController
    {
        // GET: FarmRentals

        private readonly FarmRentsReportsDomainSerices _farmRentsReportsDomainSerices;

        protected FarmRentReportsAppEntity _farmRentReportsAppEntity;
        protected FarmRentReportsApplicationService _farmRentReportsApplicationService;

        public FarmRentalsController(FarmRentsReportsDomainSerices farmRentsReportsDomainSerices)
        {
            _farmRentsReportsDomainSerices = farmRentsReportsDomainSerices;
            _farmRentReportsAppEntity = new FarmRentReportsAppEntity();
            _farmRentReportsAppEntity.farmRentsReportsDomainSerices = farmRentsReportsDomainSerices;
            _farmRentReportsApplicationService = new FarmRentReportsApplicationService(_farmRentReportsAppEntity);

        }



        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/Accounts/FarmRentals/Index.cshtml");
        }

        public ActionResult New()
        {
            string path = Request.Url.AbsolutePath;
            var urlArray = path.Split('/');
            ViewBag.AreaId = urlArray[3];//.Last();
            ViewBag.ProjectId = urlArray[4];
            return View("~/Views/Accounts/FarmRentals/New.cshtml");
        }


        public ActionResult GetAllProjectList()
        {
            IList<CreateSegmentProject> projectList = _farmRentReportsApplicationService.GetAllProjectList();
            return Json(projectList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            int feedId = id;
            FarmRentsCostsTableDomain farmRentsCostsTableDomain = new FarmRentsCostsTableDomain();
            FarmRentsCostsTable result = new FarmRentsCostsTable();

            return DataActionViewService(
               () =>
               {
                   result = _farmRentReportsApplicationService.GetFarmRentalDetails(feedId);
                   farmRentsCostsTableDomain.farmRentsCostsTable = result;
                   ViewBag.FarmRentalDate = result.FarmRentalDate;
                   farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDetails = HttpUtility.HtmlDecode(result.FarmRentalDetails);
               },
               () =>
               {
                   if (farmRentsCostsTableDomain != null)
                   {
                       return View("~/Views/Accounts/FarmRentals/Edit.cshtml", farmRentsCostsTableDomain);
                   }

                   return View("~/Views/Accounts/FarmRentals/Edit.cshtml");
               },
               () => View("~/Views/Accounts/FarmRentals/Edit.cshtml"));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFarmRentReport(FarmRentsCostsTableDomain farmRentsCostsTableDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (farmRentsCostsTableDomain != null)
                    {
                        farmRentsCostsTableDomain.isAddOperation = true;
                        farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportName = "";
                        isAddSuccess = _farmRentReportsApplicationService.ProcessFarmRentReport(farmRentsCostsTableDomain, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে হিসাবটি সংযুক্ত করেছেন");
                        return RedirectToAction("Index", "FarmRentals");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "FarmRentals");
                },
                () => View());


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFarmRentReport(FarmRentsCostsTableDomain farmRentsCostsTableDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (farmRentsCostsTableDomain != null)
                    {
                        farmRentsCostsTableDomain.isAddOperation = false;
                        farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportName = "";
                        isAddSuccess = _farmRentReportsApplicationService.ProcessFarmRentReport(farmRentsCostsTableDomain, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে হিসাবটি সম্পাদন করেছেন");
                        return RedirectToAction("Index", "FarmRentals");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "FarmRentals");
                },
                () => View());


        }


        public ActionResult GetFarmRentalReportsBySearchParam(string projectId, string areaId)
        {
            int totalRows = 0;
            IList<SPFarmRentalReportsBySearchParam> dataResult = _farmRentReportsApplicationService.GetFarmRentalReportsBySearchParam(projectId, areaId);

            totalRows = dataResult.Count;

            var result = new DataSourceResult()
            {
                Data = dataResult,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeleteFarmRentalReport(int id)
        {
            int result = _farmRentReportsApplicationService.DeleteFarmRentalReport(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}