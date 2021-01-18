using Services.ApplicationEntity.CreateSegement;
using Services.ApplicationEntity.FarmExpend;
using Services.ApplicationServices.CreateSegement.Area;
using Services.ApplicationServices.FarmExpend;
using Services.Domain.FarmExpend;
using Services.DomainServices.CreateSegement.Area;
using Services.DomainServices.FarmExpend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.Accounts
{
    public class FarmExpendController : BaseController
    {
        protected CreateSegementAppEntity _createSegementAppEntity;
        protected FarmExpendApplicationService _farmExpendApplicationService;
        protected FarmExpandAppEntity _farmExpendAppEntity;
        protected AreaSegmentApplicationService _areaSegmentApplicationService;

        public FarmExpendController(AreaSegmentDomainService areaSegmentDomainService, FarmExpendDomainService farmExpendDomainService)
        {
            _farmExpendAppEntity = new FarmExpandAppEntity();
            _createSegementAppEntity = new CreateSegementAppEntity();

            _createSegementAppEntity.areaSegmentDomainService = areaSegmentDomainService;
            _farmExpendAppEntity.farmExpendDomainService = farmExpendDomainService;

            _areaSegmentApplicationService = new AreaSegmentApplicationService(_createSegementAppEntity);
            _farmExpendApplicationService = new FarmExpendApplicationService(_farmExpendAppEntity);
        }


        // GET: FarmExpend
        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;

            return View();
        }
        public ActionResult Add(int id)
        {
           LoggedInUserInfoFromCookie.UserExpendIdInCookie = id;
           ViewBag.AllFeedListForBindUI = _areaSegmentApplicationService.GetAllAreaList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFarmExpandReport(FarmExpandTableDomain farmExpendDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int userExpendId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserExpendIdInCookie);

            List<ExpendReportMapper> areaListMapper = new List<ExpendReportMapper>();

            for (int item = 0; item < farmExpendDomain.ExpendReportMapperList.Count; item++)
            {
                ExpendReportMapper singleItem = new ExpendReportMapper();
                var thisItem = farmExpendDomain.ExpendReportMapperList[item];

                if (thisItem.ExpendAmount > 0 )
                {
                    //Assign Value in New Fish Object   
                    singleItem.ExpendMapperId = 0;
                    singleItem.ExpendReportId = 0;
                    singleItem.ExpendAmount = thisItem.ExpendAmount;
                    singleItem.AreaId = thisItem.AreaId;

                    var isAreaExist = CheckAreaIdExistInCurrentList(thisItem.AreaId.ToString(), farmExpendDomain.AreaIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isAreaExist == true)
                    {
                        areaListMapper.Add(singleItem);
                    }

                }

            }


            FarmExpandTableDomain dataModal = new FarmExpandTableDomain();
            dataModal.ExpendReportTable = new ExpendReportTable();
            dataModal.AreaIdListWithComma = farmExpendDomain.AreaIdListWithComma;
            dataModal.ExpendReportMapperList = areaListMapper;

            dataModal.ExpendReportTable.ExpendReportId = farmExpendDomain.ExpendReportTable.ExpendReportId;
            dataModal.ExpendReportTable.ExpendRepoterName = farmExpendDomain.ExpendReportTable.ExpendRepoterName;
            dataModal.ExpendReportTable.ExpendId = userExpendId;
            dataModal.ExpendReportTable.TotalExpend = farmExpendDomain.ExpendReportTable.TotalExpend;
            dataModal.ExpendReportTable.CreateDate = farmExpendDomain.ExpendReportTable.CreateDate;
            dataModal.ExpendReportTable.CreatedId = farmExpendDomain.ExpendReportTable.CreatedId;
            dataModal.ExpendReportTable.ExpandNote = farmExpendDomain.ExpendReportTable.ExpandNote;

            dataModal.isAddOperation = true;
            isAddSuccess = _farmExpendApplicationService.ProcessFarmExpendReport(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খরচের হিসাবটি সম্পাদন করেছেন");
                return RedirectToAction("Index", "FarmExpend");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "FarmExpend");
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

        public ActionResult Edit(int id)
        {
            FarmExpandTableDomain farmExpendTableDomain = new FarmExpandTableDomain();
            ViewBag.AllFeedListForBindUI = _areaSegmentApplicationService.GetAllAreaList();

            return DataActionViewService(
               () =>
               {
                   farmExpendTableDomain = _farmExpendApplicationService.GetFarmExpendDetailsByExpendID(id);
                   ViewBag.SellDate = farmExpendTableDomain.ExpendReportTable.CreateDate;
                   farmExpendTableDomain.ExpendReportTable.ExpandNote = HttpUtility.HtmlDecode(farmExpendTableDomain.ExpendReportTable.ExpandNote);
                   LoggedInUserInfoFromCookie.UserExpendIdInCookie = farmExpendTableDomain.ExpendReportTable.ExpendId;
               },
               () =>
               {
                   if (farmExpendTableDomain != null)
                   {
                       return View("~/Views/FarmExpend/Edit.cshtml", farmExpendTableDomain);
                   }
                   else
                   {
                       return View("~/Views/FarmExpend/Edit.cshtml");
                   }


               },
               () => View("~/Views/FarmExpend/Edit.cshtml"));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFarmExpandReport(FarmExpandTableDomain farmExpendDomain)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int userExpendId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserExpendIdInCookie);

            List<ExpendReportMapper> areaListMapper = new List<ExpendReportMapper>();

            for (int item = 0; item < farmExpendDomain.ExpendReportMapperList.Count; item++)
            {
                ExpendReportMapper singleItem = new ExpendReportMapper();
                var thisItem = farmExpendDomain.ExpendReportMapperList[item];

                if (thisItem.ExpendAmount > 0)
                {
                    //Assign Value in New Fish Object
                    singleItem.ExpendMapperId = 0;
                    singleItem.ExpendReportId = 0;
                    singleItem.ExpendAmount = thisItem.ExpendAmount;
                    singleItem.AreaId = thisItem.AreaId;
                  
                    var isAreaExist = CheckAreaIdExistInCurrentList(thisItem.AreaId.ToString(), farmExpendDomain.AreaIdListWithComma);
                    //Add Fish Item In Temp Fish List
                    if (isAreaExist == true)
                    {
                        areaListMapper.Add(singleItem);
                    }

                }

            }


            FarmExpandTableDomain dataModal = new FarmExpandTableDomain();
            dataModal.ExpendReportTable = new ExpendReportTable();
            dataModal.AreaIdListWithComma = farmExpendDomain.AreaIdListWithComma;
            dataModal.ExpendReportMapperList = areaListMapper;
            dataModal.ExistingMapperIdListForEdit = farmExpendDomain.ExistingMapperIdListForEdit;


            dataModal.ExpendReportTable.ExpendReportId = farmExpendDomain.ExpendReportTable.ExpendReportId;
            dataModal.ExpendReportTable.ExpendRepoterName = farmExpendDomain.ExpendReportTable.ExpendRepoterName;
            dataModal.ExpendReportTable.ExpendId = userExpendId;
            dataModal.ExpendReportTable.TotalExpend = farmExpendDomain.ExpendReportTable.TotalExpend;
            dataModal.ExpendReportTable.CreateDate = farmExpendDomain.ExpendReportTable.CreateDate;
            dataModal.ExpendReportTable.CreatedId = farmExpendDomain.ExpendReportTable.CreatedId;
            dataModal.ExpendReportTable.ExpandNote = farmExpendDomain.ExpendReportTable.ExpandNote;

            dataModal.isAddOperation = false;
            isAddSuccess = _farmExpendApplicationService.ProcessFarmExpendReport(dataModal, adminUserId);

            if (isAddSuccess == true)
            {
                MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে হিসাবটি সম্পাদন করেছেন");
                return RedirectToAction("Index", "FarmExpend");
            }
            else
            {
                MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
            }
            return RedirectToAction("Index", "FarmExpend");
        }




    }
}