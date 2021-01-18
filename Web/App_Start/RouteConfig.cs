using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            name: "default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "DownloadFeedPurchasePDF",
              url: "PDFGenerate/DownloadFeedPurchasePDF/{purchaseId}/{purchaseIdTest}",
              defaults: new { controller = "PDFGenerate", action = "DownloadFeedPurchasePDF" }
          );

            routes.MapRoute(
             name: "NewSell",
             url: "SellOfFish/NewSell/{selectedAreaId}/{selectedProjectId}",
             defaults: new { controller = "SellOfFish", action = "NewSell" }
         );

            routes.MapRoute(
             name: "Add",
             url: "FeedPurchase/Add/{selectedAreaId}/{selectedProjectId}",
             defaults: new { controller = "FeedPurchase", action = "Add" }
         );

            routes.MapRoute(
             name: "NewDistribution",
             url: "FeedDistribute/NewDistribution/{selectedAreaId}/{selectedProjectId}",
             defaults: new { controller = "FeedDistribute", action = "NewDistribution" }
         );

            routes.MapRoute(
            name: "New",
            url: "FarmRentals/New/{selectedAreaId}/{selectedProjectId}",
            defaults: new { controller = "FarmRentals", action = "New" }
        );

            routes.MapRoute(
              name: "GenerateFeedPurchasePDF",
              url: "PDFGenerate/GenerateFeedPurchasePDF/{purchaseId}/{purchaseIdTest}",
              defaults: new { controller = "PDFGenerate", action = "GenerateFeedPurchasePDF" }

          );

            routes.MapRoute(
              name: "DownloadUserFishSellPDFInfo",
              url: "PDFGenerate/DownloadUserFishSellPDFInfo/{SellId}/{SellIdTest}",
              defaults: new { controller = "PDFGenerate", action = "DownloadUserFishSellPDFInfo" }
          );
            routes.MapRoute(
              name: "GenerateUserFishSellPDF",
              url: "PDFGenerate/GenerateUserFishSellPDF/{SellId}/{SellIdTest}",
              defaults: new { controller = "PDFGenerate", action = "GenerateUserFishSellPDF" }
          );

            routes.MapRoute(
              name: "DownloadFishBillingAmountPDF",
              url: "PDFGenerate/DownloadFishBillingAmountPDF/{fishSellId}/{paidAmount}",
              defaults: new { controller = "PDFGenerate", action = "DownloadFishBillingAmountPDF" }
          );

            routes.MapRoute(
              name: "GenerateFishBillingPaidAmountPDF",
              url: "PDFGenerate/GenerateFishBillingPaidAmountPDF/{fishSellId}/{paidAmount}",
              defaults: new { controller = "PDFGenerate", action = "GenerateFishBillingPaidAmountPDF" }
          );

            routes.MapRoute(
              name: "DownLoadFishSellerFullInformation",
              url: "PDFGenerate/DownLoadFishSellerFullInformation/{SellerId}/{id}",
              defaults: new { controller = "PDFGenerate", action = "DownLoadFishSellerFullInformation" }
          );

            routes.MapRoute(
              name: "GenerateFishSellerFullOpenHisabPDF",
              url: "PDFGenerate/GenerateFishSellerFullOpenHisabPDF/{SellerId}/{id}",
              defaults: new { controller = "PDFGenerate", action = "GenerateFishSellerFullOpenHisabPDF" }
          );

            routes.MapRoute(
              name: "DownloadFishBillingHistoryBySellerId",
              url: "PDFGenerate/DownloadFishBillingHistoryBySellerId/{SellerId}/{id}",
              defaults: new { controller = "PDFGenerate", action = "DownloadFishBillingHistoryBySellerId" }
          );

            routes.MapRoute(
             name: "FishBillingHistoryForSellerIdPDF",
             url: "PDFGenerate/FishBillingHistoryForSellerIdPDF/{SellerId}/{id}",
             defaults: new { controller = "PDFGenerate", action = "FishBillingHistoryForSellerIdPDF" }
         );


            routes.MapRoute(
                name: "DownloadSellerPDFTest",
                url: "PDFGenerate/DownloadSellerPDF/{fishSellId}/{Date}",
                defaults: new { controller = "PDFGenerate", action = "DownloadSellerPDF" }

            );

            routes.MapRoute(
              name: "GenerateSellerPDFTest",
              url: "PDFGenerate/GenerateSellerPDF/{fishSellId}/{Date}",
              defaults: new { controller = "PDFGenerate", action = "GenerateSellerPDF" }

          );

            routes.MapRoute(
                name: "DownLoadFishSellingReportForAdmin",
                url: "PDFGenerate/DownLoadFishSellingReportForAdmin/{startDate}/{endDate}/{isPartial}/{areaId}/{projectId}",
                defaults: new { controller = "PDFGenerate", action = "DownLoadFishSellingReportForAdmin" }

            );

            routes.MapRoute(
              name: "GenerateFishSellingReportPDFForAdmin",
              url: "PDFGenerate/GenerateFishSellingReportPDFForAdmin/{startDate}/{endDate}/{isPartial}/{areaId}/{projectId}",
              defaults: new { controller = "PDFGenerate", action = "GenerateFishSellingReportPDFForAdmin" }

          );



            

        }
    }
}
