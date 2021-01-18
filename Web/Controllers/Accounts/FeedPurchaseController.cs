using Kendo.Mvc.UI;
using Services.ApplicationEntity.CreateSegement;
using Services.ApplicationEntity.FeedPurchase;
using Services.ApplicationServices.CreateSegement.Area;
using Services.ApplicationServices.CreateSegement.Feed;
using Services.ApplicationServices.FeedPurchase;
using Services.Domain.FeedPurchase;
using Services.DomainServices.CreateSegement.Area;
using Services.DomainServices.CreateSegement.Feed;
using Services.DomainServices.FeedPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;


namespace Web.Controllers.Accounts
{
    public class FeedPurchaseController : BaseController
    {
        // GET: FeedPurchase


        private readonly FeedPurchaseDomainService _feedPurchaseDomainService;



        protected CreateSegementAppEntity _createSegementAppEntity;
        protected FeedPurchaseAppEntity _feedPurchaseAppEntity;

        protected AreaSegmentApplicationService _areaSegmentApplicationService;
        protected FeedPurchaseApplicationService _feedPurchaseApplicationService;

        public FeedPurchaseController(AreaSegmentDomainService areaSegmentDomainService, FeedPurchaseDomainService feedPurchaseDomainService)
        {
            _feedPurchaseDomainService = feedPurchaseDomainService;


            _feedPurchaseAppEntity = new FeedPurchaseAppEntity();
            _createSegementAppEntity = new CreateSegementAppEntity();
            _createSegementAppEntity.areaSegmentDomainService = areaSegmentDomainService;
            _feedPurchaseAppEntity.feedPurchaseDomainService = feedPurchaseDomainService;

            _areaSegmentApplicationService = new AreaSegmentApplicationService(_createSegementAppEntity);
            _feedPurchaseApplicationService = new FeedPurchaseApplicationService(_feedPurchaseAppEntity);

        }


        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;

            return View();
        }


        public ActionResult Add()
        {
            //string path = Request.Url.AbsolutePath;
            //var urlArray = path.Split('/');
            //ViewBag.AreaId = urlArray[3];//.Last();
            //ViewBag.ProjectId = urlArray[4];
            ViewBag.AllFeedListForBindUI = _areaSegmentApplicationService.GetAllAreaList();
            return View();
        }

        public ActionResult Edit(int id)
        {
            FeedPurchaseTableDomain feedPurchaseTableDomain = new FeedPurchaseTableDomain();
            ViewBag.AllFeedListForBindUI = _areaSegmentApplicationService.GetAllAreaList();

            return DataActionViewService(
               () =>
               {
                   feedPurchaseTableDomain = _feedPurchaseApplicationService.GetFeedPurchaseDetailsByFeedPurchaseID(id);
                   ViewBag.SellDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCalculationDate;
                   feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedSellNote = HttpUtility.HtmlDecode(feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedSellNote);
               },
               () =>
               {
                   if (feedPurchaseTableDomain != null)
                   {
                       return View("~/Views/FeedPurchase/Edit.cshtml", feedPurchaseTableDomain);
                   }
                   else
                   {
                       return View("~/Views/FeedPurchase/Edit.cshtml");
                   }


               },
               () => View("~/Views/FeedPurchase/Edit.cshtml"));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeedPurchaseReport(FeedPurchaseTableDomain feedPurchaseTableDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            List<FeedPurchaseReportTable> areaListMapper = new List<FeedPurchaseReportTable>();

            for (int item = 0; item < feedPurchaseTableDomain.FeedPurchaseReportTable.Count; item++)
            {
                FeedPurchaseReportTable singleItem = new FeedPurchaseReportTable();
                var thisItem = feedPurchaseTableDomain.FeedPurchaseReportTable[item];

                if (thisItem.FeedBagsWeight > 0 && thisItem.TotalWeight > 0 && thisItem.PricePerKg > 0 && thisItem.TotalPrice > 0)
                {
                    //Assign Value in New Fish Object
                    singleItem.FeedSellingReportMapperId = 0;
                    singleItem.FeedSellingReportId = 0;
                    singleItem.FeedId = thisItem.FeedId;
                    singleItem.FeedCategoryId = thisItem.FeedCategoryId;
                    singleItem.FeedTotalBags = thisItem.FeedTotalBags;
                    singleItem.FeedBagsWeight = thisItem.FeedBagsWeight;
                    singleItem.TotalWeight = thisItem.TotalWeight;
                    singleItem.PricePerKg = thisItem.PricePerKg;
                    singleItem.TotalPrice = thisItem.TotalPrice;
                    singleItem.MapperAreaId = thisItem.MapperAreaId;

                    var isAreaExist = CheckAreaIdExistInCurrentList(thisItem.MapperAreaId.ToString(), feedPurchaseTableDomain.AreaIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isAreaExist == true)
                    {
                        areaListMapper.Add(singleItem);
                    }

                }

            }


            FeedPurchaseTableDomain dataModal = new FeedPurchaseTableDomain();
            dataModal.FeedPurcaseTable = new FeedPurcaseTable();
            dataModal.AreaIdListWithComma = feedPurchaseTableDomain.AreaIdListWithComma;
            dataModal.FeedPurchaseReportTable = areaListMapper;

            dataModal.FeedPurcaseTable.FeedSellingReportId = feedPurchaseTableDomain.FeedPurcaseTable.FeedSellingReportId;
            dataModal.FeedPurcaseTable.SellingFeedReportNumber = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedReportNumber;
            dataModal.FeedPurcaseTable.SellingReportName = feedPurchaseTableDomain.FeedPurcaseTable.SellingReportName;
            dataModal.FeedPurcaseTable.SellingFeedTotalWeight = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedTotalWeight;
            dataModal.FeedPurcaseTable.SellingFeedTotalPrice = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedTotalPrice;
            dataModal.FeedPurcaseTable.SellingFeedCalculationDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCalculationDate;
            dataModal.FeedPurcaseTable.SellingFeedCreateDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCreateDate;
            dataModal.FeedPurcaseTable.SellingFeedCreatedId = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCreatedId;
            dataModal.FeedPurcaseTable.SellingFeedEditedDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedEditedDate;
            dataModal.FeedPurcaseTable.SellignFeedEditedId = feedPurchaseTableDomain.FeedPurcaseTable.SellignFeedEditedId;
            dataModal.FeedPurcaseTable.SellingFeedSellNote = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedSellNote;
            dataModal.FeedPurcaseTable.FeedAmountPaid = feedPurchaseTableDomain.FeedPurcaseTable.FeedAmountPaid;
            dataModal.FeedPurcaseTable.FeedAmountDue = feedPurchaseTableDomain.FeedPurcaseTable.FeedAmountDue;
            dataModal.FeedPurcaseTable.AreaId = feedPurchaseTableDomain.FeedPurcaseTable.AreaId;
            dataModal.FeedPurcaseTable.ProjectId = feedPurchaseTableDomain.FeedPurcaseTable.ProjectId;
            dataModal.FeedPurcaseTable.IsClosedByAdmin = 0;

            dataModal.isAddOperation = true;
            isAddSuccess = _feedPurchaseApplicationService.ProcessFeedPurchaseReport(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্য ক্রয়ের হিসাবটি সম্পাদন করেছেন");
                return RedirectToAction("Index", "FeedPurchase");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "FeedPurchase");
        }


        private bool CheckAreaIdExistInCurrentList(string thisAreaId, string allAreaId)
        {

            bool returnValue = false;

            if (allAreaId != "")
            {
                var splitIdArray = allAreaId.Split(',');

                for (int item = 0; item < splitIdArray.Length; item++)
                {
                    if (splitIdArray[item] == thisAreaId)
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFeedPurchaseReport(FeedPurchaseTableDomain feedPurchaseTableDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            List<FeedPurchaseReportTable> areaListMapper = new List<FeedPurchaseReportTable>();

            for (int item = 0; item < feedPurchaseTableDomain.FeedPurchaseReportTable.Count; item++)
            {
                FeedPurchaseReportTable singleItem = new FeedPurchaseReportTable();
                var thisItem = feedPurchaseTableDomain.FeedPurchaseReportTable[item];

                if (thisItem.FeedBagsWeight > 0 && thisItem.TotalWeight > 0 && thisItem.PricePerKg > 0 && thisItem.TotalPrice > 0)
                {
                    //Assign Value in New Fish Object
                    singleItem.FeedSellingReportMapperId = 0;
                    singleItem.FeedSellingReportId = 0;
                    singleItem.FeedId = thisItem.FeedId;
                    singleItem.FeedCategoryId = thisItem.FeedCategoryId;
                    singleItem.FeedTotalBags = thisItem.FeedTotalBags;
                    singleItem.FeedBagsWeight = thisItem.FeedBagsWeight;
                    singleItem.TotalWeight = thisItem.TotalWeight;
                    singleItem.PricePerKg = thisItem.PricePerKg;
                    singleItem.TotalPrice = thisItem.TotalPrice;
                    singleItem.MapperAreaId = thisItem.MapperAreaId;

                    var isAreaExist = CheckAreaIdExistInCurrentList(thisItem.MapperAreaId.ToString(), feedPurchaseTableDomain.AreaIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isAreaExist == true)
                    {
                        areaListMapper.Add(singleItem);
                    }

                }

            }


            FeedPurchaseTableDomain dataModal = new FeedPurchaseTableDomain();
            dataModal.FeedPurcaseTable = new FeedPurcaseTable();
            dataModal.AreaIdListWithComma = feedPurchaseTableDomain.AreaIdListWithComma;
            dataModal.FeedPurchaseReportTable = areaListMapper;
            dataModal.ExistingMapperIdListForEdit = feedPurchaseTableDomain.ExistingMapperIdListForEdit;


            dataModal.FeedPurcaseTable.FeedSellingReportId = feedPurchaseTableDomain.FeedPurcaseTable.FeedSellingReportId;
            dataModal.FeedPurcaseTable.SellingFeedReportNumber = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedReportNumber;
            dataModal.FeedPurcaseTable.SellingReportName = feedPurchaseTableDomain.FeedPurcaseTable.SellingReportName;
            dataModal.FeedPurcaseTable.SellingFeedTotalWeight = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedTotalWeight;
            dataModal.FeedPurcaseTable.SellingFeedTotalPrice = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedTotalPrice;
            dataModal.FeedPurcaseTable.SellingFeedCalculationDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCalculationDate;
            dataModal.FeedPurcaseTable.SellingFeedCreateDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCreateDate;
            dataModal.FeedPurcaseTable.SellingFeedCreatedId = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedCreatedId;
            dataModal.FeedPurcaseTable.SellingFeedEditedDate = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedEditedDate;
            dataModal.FeedPurcaseTable.SellignFeedEditedId = feedPurchaseTableDomain.FeedPurcaseTable.SellignFeedEditedId;
            dataModal.FeedPurcaseTable.SellingFeedSellNote = feedPurchaseTableDomain.FeedPurcaseTable.SellingFeedSellNote;
            dataModal.FeedPurcaseTable.FeedAmountPaid = feedPurchaseTableDomain.FeedPurcaseTable.FeedAmountPaid;
            dataModal.FeedPurcaseTable.FeedAmountDue = feedPurchaseTableDomain.FeedPurcaseTable.FeedAmountDue;
            dataModal.FeedPurcaseTable.IsClosedByAdmin = 0;

            dataModal.isAddOperation = false;
            isAddSuccess = _feedPurchaseApplicationService.ProcessFeedPurchaseReport(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্য ক্রয়ের হিসাবটি সম্পাদন করেছেন");
                return RedirectToAction("Index", "FeedPurchase");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "FeedPurchase");
        }


        public ActionResult GetFeedPurchaseSearchResultByParam(string areaId, string projectId, string feedId, string catId, string calculationName)
        {
            int totalRows = 0;

            FeedPurchaseSearchResultByParam sellingReportList = _feedPurchaseApplicationService.GetFeedPurchaseSearchResultByParam(areaId, projectId, feedId, catId, calculationName);

            totalRows = sellingReportList.FeedPurchaseTableInfoList.Count;

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteFeedPurchaseRecord(int id)
        {
            int result = _feedPurchaseApplicationService.DeleteFeedPurchaseRecord(id);

            return Json(result, JsonRequestBehavior.AllowGet);



        }

        public ActionResult GetSingleFeedPurchaseReport(int purchaseId)
        {
            FeedPurchasePDFInfo singleReport = _feedPurchaseApplicationService.GetSingleFeedPurchaseReport(purchaseId);
            
            return Json(singleReport, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedPurchasePieChart()
        {
            IList<up_GetFeedPurchasePieChart> feedPurchaseList = _feedPurchaseApplicationService.GetFeedPurchasePieChart();

            return Json(feedPurchaseList, JsonRequestBehavior.AllowGet);
        }
    }

}