using Kendo.Mvc.UI;
using Services.ApplicationEntity.CostReport;
using Services.ApplicationServices.CostReport;
using Services.Domain.CostReport;
using Services.DomainServices.CostReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.CostReport
{
    public class CostReportController : BaseController
    {
        // GET: CostReport
        private readonly CostReportDomainService _costReportDomainService;

        protected CostReportAppEntity _costReportAppEntity;
        protected CostReportApplicationService _costReportApplicationService;

        public CostReportController(CostReportDomainService costReportDomainService)
        {
            _costReportDomainService = costReportDomainService;
            _costReportAppEntity = new CostReportAppEntity();
            _costReportAppEntity.costReportDomainService = costReportDomainService;
            _costReportApplicationService = new CostReportApplicationService(_costReportAppEntity);

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCost()
        {
            return View();
        }


        public ActionResult GetCostExpandNameList(string searchText)
        {
            int totalRows = 0;

            IList<CostExpandNamePorperty> dataList = _costReportApplicationService.GetCostExpandNameList(searchText);

            totalRows = dataList.Count;

            var result = new DataSourceResult()
            {
                Data = dataList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public string UploadFishImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/CostReport");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            var result = new DataSourceResult()
            {
                Data = "Uploads/CostReport/" + saVingFileName
            };
            //return Json(result, JsonRequestBehavior.AllowGet);
            return "Uploads/CostReport/" + saVingFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCostName(CostReportDomains cCostReportDomains)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (cCostReportDomains != null)
                    {
                        cCostReportDomains.costExpandTable.ID = 1;
                        cCostReportDomains.costExpandTable.IsDeleted = false;
                        cCostReportDomains.isAddOperation = true;

                        isAddSuccess = _costReportApplicationService.ProcessCostName(cCostReportDomains, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খরচটি সংযুক্ত করেছেন");
                        return RedirectToAction("Index", "CostReport");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "CostReport");
                },
                () => View());


        }



        public ActionResult Edit(int id)
        {
            CostReportDomains costReportDomains = new CostReportDomains();
            CostExpandTable result = new CostExpandTable();

            return DataActionViewService(
               () =>
               {
                   result = _costReportApplicationService.GetCostExpandNameDetails(id);
                   costReportDomains.costExpandTable = result;
                   costReportDomains.costExpandTable.Description = HttpUtility.HtmlDecode(result.Description);
               },
               () =>
               {
                   if (costReportDomains != null)
                   {
                       return View("~/Views/CostReport/Edit.cshtml", costReportDomains);
                   }

                   return View("~/Views/CostReport/Edit.cshtml");
               },
               () => View("~/Views/CostReport/Edit.cshtml"));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EitCostName(CostReportDomains costReportDomains)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (costReportDomains != null)
                    {
                        costReportDomains.isAddOperation = false;
                        isAddSuccess = _costReportApplicationService.ProcessCostName(costReportDomains, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে হিসাবটি সম্পাদন করেছেন");
                        return RedirectToAction("Index", "CostReport");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "CostReport");
                },
                () => View());


        }


        public ActionResult DeleteCostExpandName(int id)
        {
            int result = _costReportApplicationService.DeleteCostExpandName(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }







    }
}