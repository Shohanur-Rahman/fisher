using Kendo.Mvc.UI;
using Services.ApplicationEntity.Accounts;
using Services.ApplicationServices.Accounts;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.FeedDistribute;
using Services.Domain.CreateSegement.Models.Feed;
using Services.DomainServices.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.Accounts
{
    public class FeedDistributeController : BaseController
    {

        private readonly FeedDistributionDoamainService _feedDistributionDoamainSerice;

        protected FeedDistributionAppEntity _feedDistributionAppEntity;
        protected FeedDistributionApplicationService _feedDistributionApplicationService;

        public FeedDistributeController(FeedDistributionDoamainService feedDistributionDoamainService)
        {
            _feedDistributionDoamainSerice = feedDistributionDoamainService;
            _feedDistributionAppEntity = new FeedDistributionAppEntity();
            _feedDistributionAppEntity.feedDistributionDoamainServices = _feedDistributionDoamainSerice;
            _feedDistributionApplicationService = new FeedDistributionApplicationService(_feedDistributionAppEntity);

        }

        // GET: FeedDistribute
        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/Accounts/FeedDistribute/Index.cshtml");
        }


        public ActionResult GetFeedAllList()
        {

            IList<CreateSegmentFeed> feedList = _feedDistributionApplicationService.GetFeedAllList();

            return Json(feedList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedCategoryListByFeedId(int feedId)
        {
            IList<CreateSegmentFeedCategories> feedCategoryList = _feedDistributionApplicationService.GetFeedCategoryListByFeedId(feedId);

            return Json(feedCategoryList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFishSellingReportByParams(string areaId, string projectId, string feedId, string categoryId, string calculationName)
        {
          
            int totalRows = 0;

            IList<SPFeedDistributionReportsBySearchParam> sellingReportList = _feedDistributionApplicationService.GetFeedDistributionReportsBySearchParam(areaId, projectId, feedId, categoryId, calculationName);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteFeedDistributionRecord(int id)
        {
            int result = _feedDistributionApplicationService.DeleteFeedDistributionRecord(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewDistribution()
        {
            string path = Request.Url.AbsolutePath;
            var urlArray = path.Split('/');
            ViewBag.AreaId = urlArray[3];//.Last();
            ViewBag.ProjectId = urlArray[4];
            ViewBag.FeedList = _feedDistributionApplicationService.GetFeedAllList();
            ViewBag.AllFeedCategory = _feedDistributionApplicationService.GetAllFeedCategories();
            return View("~/Views/Accounts/FeedDistribute/NewDistribution.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeedDistributionReport(FeedDistributionTableDataModal feedDistributionTableDataModal)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
           
            return DataActionViewService(
                () =>
                {
                    if (feedDistributionTableDataModal != null)
                    {
                       
                        feedDistributionTableDataModal.isAddOperation = true;
                        feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionName = "খাদ্য";
                        isAddSuccess = _feedDistributionApplicationService.ProcessFeedDistributionReports(feedDistributionTableDataModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাবার খাওয়ানোর হিসাব হিসাবটি সংযুক্ত করেছেন");
                        return RedirectToAction("Index", "FeedDistribute");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "FeedDistribute");
                },
                () => View());


        }

        public ActionResult Edit(int id)
        {
            int feedId = id;
            FeedDistributionTableDataModal feedDistributionTableDataModal = new FeedDistributionTableDataModal();
            FeedDistributionTableDoamain data = new FeedDistributionTableDoamain();

            return DataActionViewService(
               () =>
               {
                   data = _feedDistributionApplicationService.GetFeedDistributionDetailsById(feedId);
                   LoggedInUserInfoFromCookie.UserAreaIdInCookie = data.AreaId;
                   LoggedInUserInfoFromCookie.UserProjectIdInCookie = data.ProjectId;
                   feedDistributionTableDataModal.feedDistributionTableDoamain = data;
                   ViewBag.FeedDistributionDate = data.FeedDistributionDate;
                   feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionDescription = HttpUtility.HtmlDecode(data.FeedDistributionDescription);
               },
               () =>
               {
                   if (feedDistributionTableDataModal != null)
                   {
                       return View("~/Views/Accounts/FeedDistribute/Edit.cshtml", feedDistributionTableDataModal);
                   }

                   return View("~/Views/Accounts/FeedDistribute/Edit.cshtml");
               },
               () => View("~/Views/Accounts/FeedDistribute/Edit.cshtml"));
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFeedDistributionReport(FeedDistributionTableDataModal feedDistributionTableDataModal)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);
            return DataActionViewService(
                () =>
                {
                    if (feedDistributionTableDataModal != null)
                    {
                        feedDistributionTableDataModal.feedDistributionTableDoamain.AreaId = areaId;
                        feedDistributionTableDataModal.feedDistributionTableDoamain.ProjectId = projectId;
                        feedDistributionTableDataModal.isAddOperation = false;
                        feedDistributionTableDataModal.feedDistributionTableDoamain.FeedDistributionName = "খাদ্য";
                        isEditSuccess = _feedDistributionApplicationService.ProcessFeedDistributionReports(feedDistributionTableDataModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাবার খাওয়ানোর হিসাবটি সম্পাদন করেছেন");
                        return RedirectToAction("Index", "FeedDistribute");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "FeedDistribute");
                },
                () => View());


        }


        public ActionResult GetFeedDistributeReportByAreaId(string areaId, string projectId)
        {
            IList<SPFeedDistributionReportsBySearchParam> sellingReportList = _feedDistributionApplicationService.GetFeedDistributeReportByAreaId(areaId,projectId);
            
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedDistributionPieChart()
        {
            IList<up_GetFeedDistributionPieChart> result = _feedDistributionApplicationService.GetFeedDistributionPieChart();

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}