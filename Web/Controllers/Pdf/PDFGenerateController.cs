using Services.ApplicationEntity.Accounts;
using Services.ApplicationEntity.FeedPurchase;
using Services.ApplicationEntity.UserBilling;
using Services.ApplicationServices.Accounts;
using Services.ApplicationServices.Admin;
using Services.ApplicationServices.FeedPurchase;
using Services.ApplicationServices.UserBilling;
using Services.DataServices.FeedPurchase;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.FeedPurchase;
using Services.Domain.PDF;
using Services.Domain.UserBilling;
using Services.DomainServices.Accounts;
using Services.DomainServices.FeedPurchase;
using Services.DomainServices.UserBilling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;
using Web.MissingReferences;

namespace Web.Controllers.Pdf
{
    public class PDFGenerateController : Controller
    {
        protected AdminDataAplicationService _adminDataAplicationService;
        protected FishSellingReportApplicationServices _fishSellingReportApplicationServices;
        protected FishSellingReportAppEntity fishSellingReportAppEntityInstance;

        protected FeedPurchaseApplicationService _feedPurchaseApplicationService;
        protected FeedPurchaseAppEntity _feedPurchaseAppEntityInstance;

        protected UserBillingApplicationService _userBillingApplicationService;
        protected UserBillingAppEntity _userBillingAppEntity;
        public PDFGenerateController(AdminDataAplicationService adminDataAplicationService, FishSellingDomainServices fishSellingDomainServices, FeedPurchaseDomainService feedPurchaseDomainService, UserBillingDomainService userBillingDomainService)
        {
            fishSellingReportAppEntityInstance = new FishSellingReportAppEntity();
            _feedPurchaseAppEntityInstance = new FeedPurchaseAppEntity();

            fishSellingReportAppEntityInstance.fishSellingDomainServices = fishSellingDomainServices;

            //Application Service: Initialize Application Service object for company user Module
            _fishSellingReportApplicationServices = new FishSellingReportApplicationServices(fishSellingReportAppEntityInstance);
            _feedPurchaseAppEntityInstance.feedPurchaseDomainService = feedPurchaseDomainService;
            _feedPurchaseApplicationService = new FeedPurchaseApplicationService(_feedPurchaseAppEntityInstance);
            _adminDataAplicationService = adminDataAplicationService;

            _userBillingAppEntity = new UserBillingAppEntity();
            _userBillingAppEntity.userBillingDomainService = userBillingDomainService;
            _userBillingApplicationService = new UserBillingApplicationService(_userBillingAppEntity);

        }
        //AdminDataAplicationService
        #region "Project PDF String For Dynamic Header & Footer

        private string dynamicFooterHTML = "<div style='width: 100%; display: block; overflow: hidden; border-top: 1px solid #000000;'>"
            + "<div style='float: left;display: block;width: 500px;margin-top: 8px;'>"
            + "<h4 style = 'margin: 0px;font-size: 11pt !important; color: #000 !important; margin-left:25px;font-family: Arial !important;'><strong style='font-weight: 600;color: #000 !important;'>Project Name: </strong><span style = 'font-weight: 200 !important;color: #000 !important;font-family: Arial !important;'>###ProjectName###</span></h4>"
            + "<h4 style='margin-top: 5px;font-size: 11pt !important; color: #000 !important;margin-left:25px;font-family: Arial !important;'><strong style='font-weight: 600;color: #000 !important;'>Reference: </strong><span style = 'font-weight: 200 !important;color: #000 !important;font-family: Arial !important;'>###ProjectReference###</span></h4>"
            + "</div>"
            + "<div  style='float: right;display: block;width: 200px;margin-top: 8px;'>"
            + "<h4 style = 'margin: 0px;font-size: 11pt !important; color: #000 !important;font-family: Arial !important;'><strong style='font-weight: 600;color: #000 !important;'>Page: </strong><span class='page' style='font-weight: 200 !important;color: #000 !important;'></span><span style='font-weight: 200 !important;color: #000 !important;'> of&nbsp;&nbsp;</span><span class='topage' style='font-weight: 200 !important;color: #000 !important;font-family: Arial !important;'></span></h4>"
            + "<h4 style='margin-top: 5px;font-size: 11pt !important; color: #000 !important;font-family: Arial !important;'><strong style='font-weight: 600;color: #000 !important;'>Date updated: </strong><span style = 'font-weight: 200 !important;color: #000 !important;font-family: Arial !important;'>###ProjectEditedDate###</span></h4>"
            + "</div>"
            + "</div>";

        #endregion

        // GET: PDFGenerate
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GenerateSellerPDF(int fishSellId, string Date)
        {
            FishSellerInfoPDF sellingReportList = _fishSellingReportApplicationServices.GetFishSellerPDFInfo(fishSellId);
            return View("~/Views/PDFGenerate/GenerateSellerPDF.cshtml", sellingReportList);
        }


        public ActionResult GetSellerPDFInfo(int fishSellId)
        {
            FishSellerInfoPDF sellingReportList = _fishSellingReportApplicationServices.GetFishSellerPDFInfo(fishSellId);
            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadSellerPDF(int fishSellId, string Date) {

            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateSellerPDF/" + fishSellId+"/"+ Date;

                #region "Dynamic Footer"
                //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (Date + "-" + guid + "seller.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                return File(mergePDFBytes, "application/pdf", fileName);
                // return RedirectToAction("ProjectReview", "PDFGenerate");
            }
            catch (Exception e) {
                throw e;
            }
        }


        public ActionResult GenerateFeedPurchasePDF(int purchaseId, int purchaseIdTest)
        {

            FeedPurchasePDFInfo singleReport = _feedPurchaseApplicationService.GetSingleFeedPurchaseReport(purchaseId);
            return View("~/Views/PDFGenerate/GenerateFeedPurchasePDF.cshtml", singleReport);
        }

        public ActionResult DownloadFeedPurchasePDF(int purchaseId, int purchaseIdTest)
        {
            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateFeedPurchasePDF/" + purchaseId + "/" + purchaseIdTest;

                FeedPurchaseTableDomain feedPurchaseTableDomain = _feedPurchaseApplicationService.GetFeedPurchaseDetailsByFeedPurchaseID(purchaseId);
                
                    #region "Dynamic Footer"
                    //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (DateTime.Now.ToString("dd-M-yyyy") + "-" + guid + "feed-purchase.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                return File(mergePDFBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ActionResult GenerateUserFishSellPDF(int SellId, int SellIdTest)
        {
            FishSellPDFInforForUser pdfInfo = _fishSellingReportApplicationServices.GetFishSellingPDFInformation(SellId);
            return View("~/Views/PDFGenerate/GenerateUserFishSellingReportPDF.cshtml", pdfInfo);
        }

        public ActionResult DownloadUserFishSellPDFInfo(int SellId, int SellIdTest)
        {
            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateUserFishSellPDF/" + SellId + "/" + SellIdTest;
                
                #region "Dynamic Footer"
                //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (DateTime.Now.ToString("dd-M-yyyy") + "-" + guid + "-user-fish-sell.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                return File(mergePDFBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ActionResult GenerateFishBillingPaidAmountPDF(string fishSellId, decimal paidAmount)
        {
            ViewBag.PaidAmount = paidAmount;
            ViewBag.OldDataList = OldDataInCurrentActionHelper.BillingAmountHistoryBySellerIdForAction;
            BillingAmountHistoryBySellerId pdfInfo = _userBillingApplicationService.GetTotalAmountPaidAmountAndDueAmount(fishSellId);
            return View("~/Views/PDFGenerate/GenerateFishBillingPaidAmountPDF.cshtml", pdfInfo);
        }

        public ActionResult DownloadFishBillingAmountPDF(string fishSellId, decimal paidAmount)
        {
            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateFishBillingPaidAmountPDF/" + fishSellId + "/" + paidAmount;

                #region "Dynamic Footer"
                //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (DateTime.Now.ToString("dd-M-yyyy") + "-" + guid + "-user-billing-sell.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                OldDataInCurrentActionHelper.BillingAmountHistoryBySellerIdForAction = null;
                return File(mergePDFBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ActionResult GenerateFishSellerFullOpenHisabPDF(string SellerId, string id)
        {
            BillingAmountHistoryBySellerId pdfInfo = _userBillingApplicationService.GetFishSellingFullReportBySellerId(SellerId);
            return View("~/Views/PDFGenerate/GenerateFishSellerFullOpenHisabPDF.cshtml", pdfInfo);
        }

        public ActionResult DownLoadFishSellerFullInformation(string SellerId, string id)
        {
            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateFishSellerFullOpenHisabPDF/" + SellerId + "/" + id;

                #region "Dynamic Footer"
                //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (DateTime.Now.ToString("dd-M-yyyy") + "-" + guid + "-user-billing-sell.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                OldDataInCurrentActionHelper.BillingAmountHistoryBySellerIdForAction = null;
                return File(mergePDFBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public ActionResult GenerateFishSellingReportPDFForAdmin(string startDate, string endDate, string isPartial, string areaId, string projectId)
        {
            areaId = areaId == "0" ? "" : areaId;
            projectId = projectId == "0" ? "" : projectId;
            startDate = startDate == "1" ? "" : startDate;
            endDate = endDate == "1" ? "" : endDate;

            FishSellingReportPDFInfo sellingReportList = _fishSellingReportApplicationServices.GetFishSellingReportPDFDataForAdmin(startDate, endDate, isPartial, areaId, projectId);
            return View("~/Views/PDFGenerate/GenerateFishSellingReportPDF.cshtml", sellingReportList);
        }

        public ActionResult DownLoadFishSellingReportForAdmin(string startDate, string endDate, string isPartial, string areaId, string projectId)
        {

            string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/GenerateFishSellingReportPDFForAdmin/" + startDate + "/" + endDate+"/"+ isPartial+"/"+ areaId+"/"+ projectId;

            #region "Dynamic Footer"
            //For Dynamic Footer
            dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "চা বাগান মৎস");
            dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
            dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
            #endregion

            string guid = Guid.NewGuid().ToString();
            string fileName = (startDate + "-" + guid + "seller.pdf").Trim();
            byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
            return File(mergePDFBytes, "application/pdf", fileName);
            // return RedirectToAction("ProjectReview", "PDFGenerate");
        }




        public ActionResult FishBillingHistoryForSellerIdPDF(int SellerId, string id)
        {
            FishBillingDownloadInfo pdfInfo = _userBillingApplicationService.GetFishSellingTotalBySellerId(SellerId);
            return View("~/Views/PDFGenerate/FishBillingHistoryForSellerId.cshtml", pdfInfo);
        }

        public ActionResult DownloadFishBillingHistoryBySellerId(string SellerId, string id)
        {
            try
            {

                string dynamicLinkForProjectPDF = Services.Utilities.SiteSettings.SiteURL + "PDFGenerate/FishBillingHistoryForSellerIdPDF/" + SellerId + "/" + id;

                #region "Dynamic Footer"
                //For Dynamic Footer
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectName###", "নবীন ট্রেডার্স");
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectReference###", LoggedInUserInfoFromCookie.UserFirstNameInCookie + " " + LoggedInUserInfoFromCookie.UserLastNameInCookie);
                dynamicFooterHTML = dynamicFooterHTML.Replace("###ProjectEditedDate###", DateTime.Now.ToString("dd MMM yy"));
                #endregion

                string guid = Guid.NewGuid().ToString();
                string fileName = (DateTime.Now.ToString("dd-M-yyyy") + "-" + guid + "-user-billing-sell.pdf").Trim();
                byte[] mergePDFBytes = _adminDataAplicationService.GetProjectPDFFileUsingNreco(dynamicLinkForProjectPDF, dynamicFooterHTML);
                OldDataInCurrentActionHelper.BillingAmountHistoryBySellerIdForAction = null;
                return File(mergePDFBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}