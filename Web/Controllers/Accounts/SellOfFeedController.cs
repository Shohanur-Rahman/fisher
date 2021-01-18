using Kendo.Mvc.UI;
using Services.ApplicationEntity.Accounts;
using Services.ApplicationServices.Accounts;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.FeedPurchase;
using Services.DomainServices.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.Accounts
{
    public class SellOfFeedController : BaseController
    {
        // GET: SellOfFeed

        private readonly FeedSellingDomainServices _feedSellingDomainServices;

        protected FeedSellingReportAppEntity _feedSellingReportAppEntity;
        protected FeedSellingReportApplicationServices _feedSellingReportApplicationServices;

        public SellOfFeedController(FeedSellingDomainServices feedSellingDomainServices)
        {
            _feedSellingDomainServices = feedSellingDomainServices;
            _feedSellingReportAppEntity = new FeedSellingReportAppEntity();
            _feedSellingReportAppEntity.feedSellingDomainServices = feedSellingDomainServices;
            _feedSellingReportApplicationServices = new FeedSellingReportApplicationServices(_feedSellingReportAppEntity);

        }


        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/Accounts/Feed/Index.cshtml");
        }

        public ActionResult NewSell()
        {
            return View("~/Views/Accounts/Feed/NewSell.cshtml");
        }


        public ActionResult GetTotalFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial)
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);
            IList<SPTotalFeedSellingReportsBySearchParam> sellingReportList = _feedSellingReportApplicationServices.GetTotalFeedSellingReportBySearchParams(startDate, endDate, catId, feedId, isPartial, areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedSellingReportBySearchParams(string startDate, string endDate, string catId, string feedId, string isPartial)
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);
            IList<SPFeedSellingReportsBySearchParam> sellingReportList = _feedSellingReportApplicationServices.GetFeedSellingReportBySearchParams(startDate, endDate, catId, feedId, isPartial, areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllFeedCategoryList()
        {
            IList<CreateSegmentFeedCategories> sellingReportList = _feedSellingReportApplicationServices.GetAllFeedCategoryList();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllFeedNameList()
        {
            IList<CreateSegmentFeed> sellingReportList = _feedSellingReportApplicationServices.GetAllFeedNameList();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedCategoryListByFeedId(int feedId)
        {
            
            IList<CreateSegmentFeedCategories> sellingReportList = _feedSellingReportApplicationServices.GetFeedCategoryListByFeedId(feedId);

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeedSellingReport(FeedSellingReportTableDataModal feedSellingReportTableDataModal)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);
            return DataActionViewService(
                () =>
                {
                    if (feedSellingReportTableDataModal != null)
                    {
                        feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedAreaId = areaId;
                        feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedProjectId = projectId;
                        feedSellingReportTableDataModal.isAddOperation = true;
                        isAddSuccess = _feedSellingReportApplicationServices.ProcessFeedSellingReport(feedSellingReportTableDataModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্য বিক্রয়ের হিসাবটি সংযুক্ত করেছেন");
                        return RedirectToAction("Index", "SellOfFeed");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "SellOfFeed");
                },
                () => View());


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFeedSellingReport(FeedSellingReportTableDataModal feedSellingReportTableDataModal)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);
            return DataActionViewService(
                () =>
                {
                    if (feedSellingReportTableDataModal != null)
                    {
                        feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedAreaId = areaId;
                        feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedProjectId = projectId;
                        feedSellingReportTableDataModal.isAddOperation = false;
                        isEditSuccess = _feedSellingReportApplicationServices.ProcessFeedSellingReport(feedSellingReportTableDataModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্য বিক্রয়ের হিসাবটি সম্পাদন করেছেন");
                        return RedirectToAction("Index", "SellOfFeed");
                    }
                    else {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "SellOfFeed");
                },
                () => View());


        }


        public ActionResult EditSell(int id)
        {
            int feedId = id;
            FeedSellingReportTableDataModal feedSellingReportTableDataModal = new FeedSellingReportTableDataModal();
            FeedSellingReportTableDomain sellingReport = new FeedSellingReportTableDomain();

            return DataActionViewService(
               () =>
               {
                   sellingReport = _feedSellingReportApplicationServices.GetFeedSellingReportDetailsById(feedId);
                   feedSellingReportTableDataModal.feedSellingReportTableDomain = sellingReport;
                   ViewBag.SellingFeedCalculationDate = sellingReport.SellingFeedCalculationDate;
                   feedSellingReportTableDataModal.feedSellingReportTableDomain.SellingFeedSellNote = HttpUtility.HtmlDecode(sellingReport.SellingFeedSellNote);
               },
               () =>
               {
                   if (feedSellingReportTableDataModal != null)
                   {
                       return View("~/Views/Accounts/Feed/EditSell.cshtml", feedSellingReportTableDataModal);
                   }

                   return View("~/Views/Accounts/Feed/EditSell.cshtml");
               },
               () => View("~/Views/Accounts/Feed/EditSell.cshtml"));
        }

        public ActionResult DeleteFeedSellingRecord(int id)
        {
            int result = _feedSellingReportApplicationServices.DeleteFeedSellingRecord(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedSellingDashboardOverview()
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            IList<SPFeedSellingUserDashboardOverview> sellingReportList = _feedSellingReportApplicationServices.GetFeedSellingDashboardOverview(areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedBuyingReportGroupByFeedCategory()
        {
            int totalRows = 0;
            int areaId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserAreaIdInCookie);
            int projectId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserProjectIdInCookie);

            IList<SPFeedBuyingReportByFeedCategory> sellingReportList = _feedSellingReportApplicationServices.GetFeedBuyingReportGroupByFeedCategory(areaId, projectId);

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedBuyingChartForAdmin()
        {
            int totalRows = 0;
            IList<SPFeedSellingUserDashboardOverview> sellingReportList = _feedSellingReportApplicationServices.GetFeedBuyingChartForAdmin();

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedBuyingReportGroupByFeedCategoryForAdmin()
        {
            int totalRows = 0;
            IList<SPFeedBuyingReportByFeedCategory> sellingReportList = _feedSellingReportApplicationServices.GetFeedBuyingReportGroupByFeedCategoryForAdmin();

            totalRows = sellingReportList.Count;

            var result = new DataSourceResult()
            {
                Data = sellingReportList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedPurchaseReportByAreaId(string areaId)
        {
            IList<up_GetFeedPurchaseReportByAreaId> sellingReportList = _feedSellingReportApplicationServices.GetFeedPurchaseReportByAreaId(areaId);
            
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FirstPageGetFeedPurchaseReportByAreaId(string areaId)
        {
           FirstPageFeedPurchaseTableTotal sellingReportList = _feedSellingReportApplicationServices.FirstPageGetFeedPurchaseReportByAreaId(areaId);

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }




    }
}