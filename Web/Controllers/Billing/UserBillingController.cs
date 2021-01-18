using Services.ApplicationEntity.UserBilling;
using Services.ApplicationServices.UserBilling;
using Services.Domain.UserBilling;
using Services.DomainServices.UserBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;
using Web.MissingReferences;

namespace Web.Controllers.Billing
{
    public class UserBillingController : BaseController
    {
        // GET: UserBilling
        private readonly UserBillingDomainService _userBillingDomainService;

        protected UserBillingAppEntity _userBillingAppEntity;
        protected UserBillingApplicationService _userBillingApplicationService;

        public UserBillingController(UserBillingDomainService userBillingDomainService)
        {
            _userBillingDomainService = userBillingDomainService;
            _userBillingAppEntity = new UserBillingAppEntity();
            _userBillingAppEntity.userBillingDomainService = userBillingDomainService;
            _userBillingApplicationService = new UserBillingApplicationService(_userBillingAppEntity);

        }

        public ActionResult BillingFish()
        {
            return View();
        }

        public ActionResult FishBillingHistory()
        {
            return View();
        }

        public ActionResult FeedBilling()
        {
            return View();
        }

        public ActionResult GetFishSellingListForBilling(string isPartial, string sellerID)
        {

            FishSellingDueBillingInfo sellingReportList = _userBillingApplicationService.GetFishSellingListForBilling(isPartial, sellerID);
            
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalAmountPaidAmountAndDueAmount(string fishSellId)
        {

            BillingAmountHistoryBySellerId sellingReportList = _userBillingApplicationService.GetTotalAmountPaidAmountAndDueAmount(fishSellId);
            OldDataInCurrentActionHelper.BillingAmountHistoryBySellerIdForAction = sellingReportList;
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateFishSellingBilling(string fishSellerId, string paidAmount)
        {
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);
            var result = _userBillingApplicationService.UpdateFishSellingBilling(fishSellerId, paidAmount, adminUserId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetBillingFishSellingSearchResult(string sellerId, string startDate,string endDate, string isPartial)
        {
            IList<FishSellingBillingHistoryList> result = _userBillingApplicationService.GetBillingFishSellingSearchResult(sellerId, startDate, endDate, isPartial);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFishSellingFullReportBySellerId(string fishSellId)
        {
            BillingAmountHistoryBySellerId result = _userBillingApplicationService.GetFishSellingFullReportBySellerId(fishSellId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedBillingReportByFeedId(string feedId)
        {
            FeedBillingReport result = _userBillingApplicationService.GetFeedBillingReportByFeedId(feedId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFishSellingTotalBySellerId(int sellerId)
        {
            FishBillingDownloadInfo result = _userBillingApplicationService.GetFishSellingTotalBySellerId(sellerId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

    }
}