using Kendo.Mvc.UI;
using Services.ApplicationEntity.Accounts;
using Services.ApplicationServices.Accounts;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Fish;
using Services.DomainServices.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.Accounts
{
    public class SellOfFishController : BaseController
    {
        // GET: SellOfFish

        private readonly FishSellingDomainServices _fishSellingDomainServices;

        protected FishSellingReportAppEntity _fishSellingReportAppEntity;
        protected FishSellingReportApplicationServices _fishSellingReportApplicationServices;

        public SellOfFishController(FishSellingDomainServices fishSellingDomainServices)
        {
            _fishSellingDomainServices = fishSellingDomainServices;
            _fishSellingReportAppEntity = new FishSellingReportAppEntity();
            _fishSellingReportAppEntity.fishSellingDomainServices = fishSellingDomainServices;
            _fishSellingReportApplicationServices = new FishSellingReportApplicationServices(_fishSellingReportAppEntity);

        }

        public ActionResult Index()
        {
            ViewBag.SelectedFishMultiSelectList = null;
            ViewBag.FishListFoMlt = new SelectList(_fishSellingReportApplicationServices.GetAllFishSellerNameList(), "ID", "Name");

            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/Accounts/Fish/Index.cshtml");
        }

        public ActionResult GetTotalFishSellingCalculationReport(string areaId, string projectId, string calCulationName, string isPartial)
        {
            int totalRows = 0;
            IList<SPFishSellingReportGetTotalRecord> sellingReportList = _fishSellingReportApplicationServices.GetTotalFishSellingCalculationReport(areaId, projectId, calCulationName, isPartial);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetFishSellingReportByParams(string areaId, string projectId, string sellerId, string isPartial)
        {
            int totalRows = 0;

            IList<SPFishSellingReportListBySearchParam> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingReportByParams(areaId, projectId, sellerId, isPartial);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeleteFishSellingRecord(int id)
        {
            int result = _fishSellingReportApplicationServices.DeleteFishSellingRecord(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewSell()
        {
            string path = Request.Url.AbsolutePath;
            var urlArray= path.Split('/');
            ViewBag.AreaId = urlArray[3];//.Last();
            ViewBag.ProjectId = urlArray[4];
            ViewBag.AllFishNameList = _fishSellingReportApplicationServices.GetAllFishList();
            return View("~/Views/Accounts/Fish/NewSell.cshtml");
        }

        public ActionResult GetAllFishList()
        {
            IList<CreateSegmentFish> sellingReportList = _fishSellingReportApplicationServices.GetAllFishList();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllFishSellerNameList()
        {
            IList<CreateSegmentFishSeller> result = _fishSellingReportApplicationServices.GetAllFishSellerNameList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSell(int id)
        {
            int feedId = id;
           // FishSellingReportTableDataModal fishSellingReportTableDataModal = new FishSellingReportTableDataModal();
            FishSellingReportTableDataModal sellingReport = new FishSellingReportTableDataModal();
            ViewBag.AllFishNameList = _fishSellingReportApplicationServices.GetAllFishList();
            return DataActionViewService(
               () =>
               {
                   sellingReport = _fishSellingReportApplicationServices.GetFishSellingDataReports(feedId);
                   ViewBag.SellDate = sellingReport.FishSellingReportTableDomain.SellDate;
                   ViewBag.AreaId = sellingReport.FishSellingReportTableDomain.AreaId;
                   ViewBag.ProjectId = sellingReport.FishSellingReportTableDomain.ProjectId;
                   sellingReport.FishSellingReportTableDomain.SellNote = HttpUtility.HtmlDecode(sellingReport.FishSellingReportTableDomain.SellNote);
               },
               () =>
               {
                   if (sellingReport != null)
                   {
                       return View("~/Views/Accounts/Fish/EditSell.cshtml", sellingReport);
                   }

                   return View("~/Views/Accounts/Fish/EditSell.cshtml");
               },
               () => View("~/Views/Accounts/Fish/EditSell.cshtml"));
            
        }


        private bool CheckFishIdExistInCurrentList(string thisFishId, string allFishId) {

            bool returnValue = false;

            if (allFishId != "") {
                var splitIdArray = allFishId.Split(',');

                for (int item = 0; item < splitIdArray.Length; item++)
                {
                    if (splitIdArray[item] == thisFishId)
                    {
                        returnValue = true;
                    }
                }
            }
           
            return returnValue;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFishSellingCalculationTemp(FishSellingReportTableDataModal fishSellingReportTableDataModal)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            //int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            //int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            List<FishSellingReportMapperTable> fishItems = new List<FishSellingReportMapperTable>();

            for (int item = 0; item < fishSellingReportTableDataModal.FishSellingMapperList.Count; item++)
            {
                FishSellingReportMapperTable singleFish = new FishSellingReportMapperTable();
                var thisItem = fishSellingReportTableDataModal.FishSellingMapperList[item];

                if (thisItem.PricePerKG > 0 || thisItem.TotalFishkg > 0 || thisItem.TotalSellPrice > 0)
                {
                    //Assign Value in New Fish Object
                    singleFish.FishSellMapperId = 0;
                    singleFish.FishSellReportId = 0;
                    singleFish.FishSellerId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellerId;
                    singleFish.SellFishId = thisItem.SellFishId;
                    singleFish.TotalFishkg = thisItem.TotalFishkg;
                    singleFish.PiecesPerKG = thisItem.PiecesPerKG;
                    singleFish.TotalPiecesFish = thisItem.TotalPiecesFish;
                    singleFish.PricePerKG = thisItem.PricePerKG;
                    singleFish.TotalSellPrice = thisItem.TotalSellPrice;

                    var isFishValid = CheckFishIdExistInCurrentList(thisItem.SellFishId.ToString(), fishSellingReportTableDataModal.FishIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isFishValid == true) {
                        fishItems.Add(singleFish);
                    }
                   
                }
                
            }


            FishSellingReportTableDataModal dataModal = new FishSellingReportTableDataModal();
            dataModal.FishSellingReportTableDomain = new FishSellingReportTableDomain();
            dataModal.FishIdListWithComma = fishSellingReportTableDataModal.FishIdListWithComma;
            dataModal.FishSellingMapperList = fishItems;

            dataModal.FishSellingReportTableDomain.FishSellId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellId;
            dataModal.FishSellingReportTableDomain.HisabName = fishSellingReportTableDataModal.FishSellingReportTableDomain.HisabName;
            dataModal.FishSellingReportTableDomain.GarirName = fishSellingReportTableDataModal.FishSellingReportTableDomain.GarirName;
            dataModal.FishSellingReportTableDomain.TotalSellInKG = fishSellingReportTableDataModal.FishSellingReportTableDomain.TotalSellInKG;
            dataModal.FishSellingReportTableDomain.SellDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.SellDate;
            dataModal.FishSellingReportTableDomain.FishSellerId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellerId;
            dataModal.FishSellingReportTableDomain.TotalSellPrice = fishSellingReportTableDataModal.FishSellingReportTableDomain.TotalSellPrice;
            dataModal.FishSellingReportTableDomain.CalculatedDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculatedDate;
            dataModal.FishSellingReportTableDomain.CalculatedById = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculatedById;
            dataModal.FishSellingReportTableDomain.CalculationEditedDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculationEditedDate;
            dataModal.FishSellingReportTableDomain.CalCulationEditedId = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalCulationEditedId;
            dataModal.FishSellingReportTableDomain.SellNote = fishSellingReportTableDataModal.FishSellingReportTableDomain.SellNote;
            dataModal.FishSellingReportTableDomain.AreaId = fishSellingReportTableDataModal.FishSellingReportTableDomain.AreaId;
            dataModal.FishSellingReportTableDomain.ProjectId = fishSellingReportTableDataModal.FishSellingReportTableDomain.ProjectId;
            dataModal.FishSellingReportTableDomain.FishAmountPaid = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishAmountPaid;
            dataModal.FishSellingReportTableDomain.FishAmountDue = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishAmountDue;
            dataModal.FishSellingReportTableDomain.IsClosedByAdmin = 0;

            dataModal.isAddOperation = true;
            isAddSuccess = _fishSellingReportApplicationServices.ProcessFishSellingCalculation(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে মাছ বিক্রয়ের হিসাবটি সংযুক্ত করেছেন");
                return RedirectToAction("Index", "SellOfFish");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "SellOfFish");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFishSellingReport(FishSellingReportTableDataModal fishSellingReportTableDataModal)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            List<FishSellingReportMapperTable> fishItems = new List<FishSellingReportMapperTable>();

            for (int item = 0; item < fishSellingReportTableDataModal.FishSellingMapperList.Count; item++)
            {
                FishSellingReportMapperTable singleFish = new FishSellingReportMapperTable();
                var thisItem = fishSellingReportTableDataModal.FishSellingMapperList[item];

                if (thisItem.PricePerKG > 0 || thisItem.TotalFishkg > 0 || thisItem.TotalSellPrice > 0)
                {
                    //Assign Value in New Fish Object
                    singleFish.FishSellMapperId = 0;
                    singleFish.FishSellReportId = 0;
                    singleFish.FishSellerId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellerId;
                    singleFish.SellFishId = thisItem.SellFishId;
                    singleFish.TotalFishkg = thisItem.TotalFishkg;
                    singleFish.PiecesPerKG = thisItem.PiecesPerKG;
                    singleFish.TotalPiecesFish = thisItem.TotalPiecesFish;
                    singleFish.PricePerKG = thisItem.PricePerKG;
                    singleFish.TotalSellPrice = thisItem.TotalSellPrice;

                    var isFishValid = CheckFishIdExistInCurrentList(thisItem.SellFishId.ToString(), fishSellingReportTableDataModal.FishIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isFishValid == true)
                    {
                        fishItems.Add(singleFish);
                    }

                }

            }


            FishSellingReportTableDataModal dataModal = new FishSellingReportTableDataModal();
            dataModal.FishSellingReportTableDomain = new FishSellingReportTableDomain();
            dataModal.FishIdListWithComma = fishSellingReportTableDataModal.FishIdListWithComma;
            dataModal.FishSellingMapperList = fishItems;
            dataModal.ExistingMapperIdListForEdit= fishSellingReportTableDataModal.ExistingMapperIdListForEdit;

            dataModal.FishSellingReportTableDomain.FishSellId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellId;
            dataModal.FishSellingReportTableDomain.HisabName = fishSellingReportTableDataModal.FishSellingReportTableDomain.HisabName;
            dataModal.FishSellingReportTableDomain.GarirName = fishSellingReportTableDataModal.FishSellingReportTableDomain.GarirName;
            dataModal.FishSellingReportTableDomain.TotalSellInKG = fishSellingReportTableDataModal.FishSellingReportTableDomain.TotalSellInKG;
            dataModal.FishSellingReportTableDomain.SellDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.SellDate;
            dataModal.FishSellingReportTableDomain.FishSellerId = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishSellerId;
            dataModal.FishSellingReportTableDomain.TotalSellPrice = fishSellingReportTableDataModal.FishSellingReportTableDomain.TotalSellPrice;
            dataModal.FishSellingReportTableDomain.CalculatedDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculatedDate;
            dataModal.FishSellingReportTableDomain.CalculatedById = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculatedById;
            dataModal.FishSellingReportTableDomain.CalculationEditedDate = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalculationEditedDate;
            dataModal.FishSellingReportTableDomain.CalCulationEditedId = fishSellingReportTableDataModal.FishSellingReportTableDomain.CalCulationEditedId;
            dataModal.FishSellingReportTableDomain.SellNote = fishSellingReportTableDataModal.FishSellingReportTableDomain.SellNote;
            dataModal.FishSellingReportTableDomain.AreaId = areaId;
            dataModal.FishSellingReportTableDomain.ProjectId = fishSellingReportTableDataModal.FishSellingReportTableDomain.ProjectId;
            dataModal.FishSellingReportTableDomain.FishAmountPaid = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishAmountPaid;
            dataModal.FishSellingReportTableDomain.FishAmountDue = fishSellingReportTableDataModal.FishSellingReportTableDomain.FishAmountDue;

            dataModal.isAddOperation = false;
            isAddSuccess = _fishSellingReportApplicationServices.ProcessFishSellingCalculation(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে মাছ বিক্রয়ের হিসাবটি সম্পাদন করেছেন");
                return RedirectToAction("Index", "SellOfFish");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "SellOfFish");
        }

        
        public ActionResult GetFishSellingDashboardOverview()
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            IList<SPFishSellingUserDashboardOverview> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingDashboardOverview(areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFishSellingReportGroupByFishName()
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            IList<SPFishSellingReportGroupByFishName> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingReportGroupByFishName(areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFishSellingChartReportForAdmin()
        {
            int totalRows = 0;
            IList<SPFishSellingUserDashboardOverview> sellingReportList = _fishSellingReportApplicationServices.GetFishSellingChartReportForAdmin();

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFishSellingPDFInformation(int sellId)
        {

            FishSellPDFInforForUser pdfInfo = _fishSellingReportApplicationServices.GetFishSellingPDFInformation(sellId);           
            return Json(pdfInfo, JsonRequestBehavior.AllowGet);
        }

    }
}