using Core.DataService;
using Services.DataContext;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.FeedPurchase;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.FeedPurchase
{
    public class FeedPurchaseDataService : EntityContextDataService<FeedPurchaseTableDomain>
    {
        public FeedPurchaseDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public bool ProcessFeedPurchaseReport(FeedPurchaseTableDomain feedPurchaseReport, int adminUserId)
        {


            if (feedPurchaseReport != null && feedPurchaseReport.isAddOperation == true)
            {
                FeedPurcaseTable sellingReport = new FeedPurcaseTable();
                sellingReport.FeedSellingReportId = feedPurchaseReport.FeedPurcaseTable.FeedSellingReportId;
                sellingReport.SellingFeedReportNumber = feedPurchaseReport.FeedPurcaseTable.SellingFeedReportNumber;
                sellingReport.SellingReportName = feedPurchaseReport.FeedPurcaseTable.SellingReportName;
                sellingReport.SellingFeedTotalWeight = feedPurchaseReport.FeedPurcaseTable.SellingFeedTotalWeight;
                sellingReport.SellingFeedTotalPrice = feedPurchaseReport.FeedPurcaseTable.SellingFeedTotalPrice;
                sellingReport.SellingFeedCalculationDate = feedPurchaseReport.FeedPurcaseTable.SellingFeedCalculationDate;              
                sellingReport.SellingFeedSellNote = feedPurchaseReport.FeedPurcaseTable.SellingFeedSellNote;
                sellingReport.IsClosedByAdmin = feedPurchaseReport.FeedPurcaseTable.IsClosedByAdmin;
                sellingReport.FeedAmountPaid = feedPurchaseReport.FeedPurcaseTable.FeedAmountPaid;
                sellingReport.FeedAmountDue = feedPurchaseReport.FeedPurcaseTable.FeedAmountDue;
                sellingReport.AreaId = feedPurchaseReport.FeedPurcaseTable.AreaId;
                sellingReport.ProjectId = feedPurchaseReport.FeedPurcaseTable.ProjectId;

                sellingReport.SellingFeedCreateDate = DateTime.Now;
                sellingReport.SellingFeedCreatedId = adminUserId;
                DbContext.Set<FeedPurcaseTable>().Add(sellingReport);
                DbContext.SaveChanges();

                FeedPurchaseDueReportMapperTable dueCalculation = new FeedPurchaseDueReportMapperTable();
                dueCalculation.FeedPurchaseDueReportMapperId = 0;
                dueCalculation.FeedPurchaseId = sellingReport.FeedSellingReportId;
                dueCalculation.FeedPurchaseAmountDue = sellingReport.FeedAmountDue;

                DbContext.Set<FeedPurchaseDueReportMapperTable>().Add(dueCalculation);
                DbContext.SaveChanges();

                if (feedPurchaseReport.FeedPurchaseReportTable != null && feedPurchaseReport.FeedPurchaseReportTable.Count > 0)
                {
                    for (int item = 0; item < feedPurchaseReport.FeedPurchaseReportTable.Count; item++)
                    {
                        FeedPurchaseReportTable singleItem = new FeedPurchaseReportTable();
                        var thisItem = feedPurchaseReport.FeedPurchaseReportTable[item];

                        if (thisItem.FeedBagsWeight > 0 && thisItem.TotalWeight > 0 && thisItem.PricePerKg > 0 && thisItem.TotalPrice > 0)
                        {
                            //Assign Value in New Fish Object
                            singleItem.FeedSellingReportMapperId = 0;
                            singleItem.FeedSellingReportId = sellingReport.FeedSellingReportId;
                            singleItem.FeedId = thisItem.FeedId;
                            singleItem.FeedCategoryId = thisItem.FeedCategoryId;
                            singleItem.FeedTotalBags = thisItem.FeedTotalBags;
                            singleItem.FeedBagsWeight = thisItem.FeedBagsWeight;
                            singleItem.TotalWeight = thisItem.TotalWeight;
                            singleItem.PricePerKg = thisItem.PricePerKg;
                            singleItem.TotalPrice = thisItem.TotalPrice;
                            singleItem.MapperAreaId = thisItem.MapperAreaId;

                            DbContext.Set<FeedPurchaseReportTable>().Add(singleItem);
                            DbContext.SaveChanges();
                        }

                    }

                }

                return true;
            }

            if (feedPurchaseReport != null && feedPurchaseReport.isAddOperation == false)
            {
                FeedPurcaseTable sellingReport = DbContext.Set<FeedPurcaseTable>().Where(x => x.FeedSellingReportId == feedPurchaseReport.FeedPurcaseTable.FeedSellingReportId).FirstOrDefault();

                FeedPurchaseDueReportMapperTable dueAmountRow = DbContext.Set<FeedPurchaseDueReportMapperTable>().Where(x => x.FeedPurchaseId == feedPurchaseReport.FeedPurcaseTable.FeedSellingReportId).FirstOrDefault();


                sellingReport.SellingFeedReportNumber = feedPurchaseReport.FeedPurcaseTable.SellingFeedReportNumber;
                sellingReport.SellingReportName = feedPurchaseReport.FeedPurcaseTable.SellingReportName;
                sellingReport.SellingFeedTotalWeight = feedPurchaseReport.FeedPurcaseTable.SellingFeedTotalWeight;
                sellingReport.SellingFeedTotalPrice = feedPurchaseReport.FeedPurcaseTable.SellingFeedTotalPrice;
                sellingReport.SellingFeedCalculationDate = feedPurchaseReport.FeedPurcaseTable.SellingFeedCalculationDate;
                sellingReport.SellingFeedSellNote = feedPurchaseReport.FeedPurcaseTable.SellingFeedSellNote;
                sellingReport.FeedAmountPaid = feedPurchaseReport.FeedPurcaseTable.FeedAmountPaid;
                sellingReport.FeedAmountDue = feedPurchaseReport.FeedPurcaseTable.FeedAmountDue;


                sellingReport.SellingFeedEditedDate = DateTime.Now;
                sellingReport.SellignFeedEditedId = adminUserId;

                dueAmountRow.FeedPurchaseAmountDue = feedPurchaseReport.FeedPurcaseTable.FeedAmountDue;


                if (feedPurchaseReport.ExistingMapperIdListForEdit != "") {
                    var splitIdArray = feedPurchaseReport.ExistingMapperIdListForEdit.Split(',');
                    if (splitIdArray.Length > 0)
                    {
                        for (int item = 0; item < splitIdArray.Length; item++)
                        {
                            var mapId = splitIdArray[item];
                            int result = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FeedSellingReportMapper] where FeedSellingReportMapperId = " + mapId + ";");
                        }
                    }

                }

                if (feedPurchaseReport.FeedPurchaseReportTable != null && feedPurchaseReport.FeedPurchaseReportTable.Count > 0)
                {
                    for (int item = 0; item < feedPurchaseReport.FeedPurchaseReportTable.Count; item++)
                    {
                        FeedPurchaseReportTable singleItem = new FeedPurchaseReportTable();
                        var thisItem = feedPurchaseReport.FeedPurchaseReportTable[item];

                        if (thisItem.FeedBagsWeight > 0 && thisItem.TotalWeight > 0 && thisItem.PricePerKg > 0 && thisItem.TotalPrice > 0)
                        {
                            //Assign Value in New Fish Object
                            singleItem.FeedSellingReportMapperId = 0;
                            singleItem.FeedSellingReportId = sellingReport.FeedSellingReportId;
                            singleItem.FeedId = thisItem.FeedId;
                            singleItem.FeedCategoryId = thisItem.FeedCategoryId;
                            singleItem.FeedTotalBags = thisItem.FeedTotalBags;
                            singleItem.FeedBagsWeight = thisItem.FeedBagsWeight;
                            singleItem.TotalWeight = thisItem.TotalWeight;
                            singleItem.PricePerKg = thisItem.PricePerKg;
                            singleItem.TotalPrice = thisItem.TotalPrice;
                            singleItem.MapperAreaId = thisItem.MapperAreaId;

                            DbContext.Set<FeedPurchaseReportTable>().Add(singleItem);
                            DbContext.SaveChanges();
                        }

                    }

                }

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public FeedPurchaseSearchResultByParam GetFeedPurchaseSearchResultByParam(string areaId, string projectId, string feedId, string catId, string calculationName)
        {

            SqlParameter prmCatId = new SqlParameter("@catId", catId);
            SqlParameter prmCalculationName = new SqlParameter("@calculationName", calculationName);
            SqlParameter prmFeedId = new SqlParameter("@feedId", feedId);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);
            FeedPurchaseSearchResultByParam feedPurchaseResult = new FeedPurchaseSearchResultByParam();
            
            SqlParameter prmCatIdTotal = new SqlParameter("@catId", catId);
            SqlParameter prmCalculationNameTotal = new SqlParameter("@calculationName", calculationName);
            SqlParameter prmFeedIdTotal = new SqlParameter("@feedId", feedId);
            SqlParameter prmAreaIdTotal = new SqlParameter("@areaId", areaId);
            SqlParameter prmProjectIdTotal = new SqlParameter("@projectId", projectId);

            List<FeedPurchaseTableInfoList> feedPurchaseReportList = DbContext.Database.SqlQuery<FeedPurchaseTableInfoList>("up_GetFeedPurchaseReportBySearchParam @feedId,@catId,@areaId,@projectId,@calculationName", prmFeedId, prmCatId,prmAreaId, prmProjectId, prmCalculationName).ToList();
            FeedPurchaseTableTotal totalPurchaseReport = DbContext.Database.SqlQuery<FeedPurchaseTableTotal>("up_GetTotalFeedPurchaseReportBySearchParam @feedId,@catId,@areaId,@projectId,@calculationName", prmFeedIdTotal, prmCatIdTotal,prmAreaIdTotal, prmProjectIdTotal, prmCalculationNameTotal).FirstOrDefault();

            feedPurchaseResult.FeedPurchaseTableInfoList = feedPurchaseReportList;
            feedPurchaseResult.FeedPurchaseTableTotal = totalPurchaseReport;

            return feedPurchaseResult;
        }


        private int CheckAreaIdExistInCurrentList(string thisAreaId, string allAreaId)
        {

            int returnValue = 0;

            if (allAreaId != "")
            {
                var splitIdArray = allAreaId.Split(',');

                for (int item = 0; item < splitIdArray.Length; item++)
                {
                    if (splitIdArray[item] == thisAreaId)
                    {
                        returnValue = item + 1;
                    }
                }
            }

            return returnValue;
        }

        public FeedPurchaseTableDomain GetFeedPurchaseDetailsByFeedPurchaseID(int id)
        {
            FeedPurchaseTableDomain sellingReport = new FeedPurchaseTableDomain();
            FeedPurcaseTable sellingOverview = new FeedPurcaseTable();
            List<FeedPurchaseReportTable> fishDetailsMapperList = new List<FeedPurchaseReportTable>();
            List<CreateSegmentArea> areaNameList = new List<CreateSegmentArea>();
              
            sellingOverview = DbContext.Set<FeedPurcaseTable>().Where(x => x.FeedSellingReportId == id).FirstOrDefault();
            fishDetailsMapperList = DbContext.Set<FeedPurchaseReportTable>().Where(x => x.FeedSellingReportId == id).ToList();
            areaNameList = DbContext.Set<CreateSegmentArea>().Where(x => x.ID != 0).ToList();

            sellingReport.FeedPurcaseTable = sellingOverview;



            string fishIdList = "";
            string existingMapperID = "";

            if (fishDetailsMapperList.Count > 0)
            {
                for (int item = 0; item < fishDetailsMapperList.Count; item++)
                {
                    if (fishIdList == "")
                    {
                        fishIdList = fishDetailsMapperList[item].MapperAreaId.ToString();
                    }
                    else
                    {
                        fishIdList = fishIdList + "," + fishDetailsMapperList[item].MapperAreaId.ToString();
                    }

                    if (existingMapperID == "")
                    {
                        existingMapperID = fishDetailsMapperList[item].FeedSellingReportMapperId.ToString();
                    }
                    else
                    {
                        existingMapperID = existingMapperID + "," + fishDetailsMapperList[item].FeedSellingReportMapperId.ToString();
                    }
                }
            }
            List<FeedPurchaseReportTable> mapperList = new List<FeedPurchaseReportTable>();

            for (int item = 0; item < areaNameList.Count; item++)
            {

                var indexNumber = CheckAreaIdExistInCurrentList(areaNameList[item].ID.ToString(), fishIdList);
                FeedPurchaseReportTable singleItem = new FeedPurchaseReportTable();

                if (indexNumber >= 1)
                {
                    mapperList.Add(fishDetailsMapperList[indexNumber - 1]);
                }
                else
                {
                    singleItem.FeedSellingReportMapperId = 0;
                    singleItem.FeedSellingReportId = 0;
                    singleItem.FeedId = 0;
                    singleItem.FeedCategoryId = 0;
                    singleItem.FeedTotalBags =0;
                    singleItem.FeedBagsWeight = 0;
                    singleItem.TotalWeight = 0;
                    singleItem.PricePerKg = 0;
                    singleItem.TotalPrice = 0;
                    singleItem.MapperAreaId = 0;

                    mapperList.Add(singleItem);
                }

            }

            sellingReport.FeedPurchaseReportTable = mapperList;
            //FishSellingReportTableDomain tempFishOverViewList = new FishSellingReportTableDomain();

            sellingReport.AreaIdListWithComma = fishIdList;
            sellingReport.ExistingMapperIdListForEdit = existingMapperID;

            return sellingReport;
        }

        public int DeleteFeedPurchaseRecord(int id)
        {
            int result =0;

            int dueReportCount = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FeedPurchaseDueReportMapper] where FeedPurchaseId = " + id + ";");
            int feedMapperCount = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FeedSellingReportMapper] where feedsellingReportid = " + id + ";");
            int purchaseCount = DbContext.Database.ExecuteSqlCommand("DELETE FROM FeedSellingReport where FeedSellingReportId = " + id);

            result = (dueReportCount + feedMapperCount + purchaseCount);

            return result;
        }


        public FeedPurchasePDFInfo GetSingleFeedPurchaseReport(int purchaseId)
        {
            SqlParameter prmReportId = new SqlParameter("@reportId", purchaseId);
            SqlParameter prmReportIdTotal = new SqlParameter("@reportId", purchaseId);
            FeedPurchasePDFInfo pdfInfo = new FeedPurchasePDFInfo();
            List<SingleFeedPurchaseReport> singleReportList = DbContext.Database.SqlQuery<SingleFeedPurchaseReport>("up_GetFeedPurchaseSingleReport @reportId", prmReportId).ToList();
            TotalFeedPurchaseReportForEachId totalReport = DbContext.Database.SqlQuery<TotalFeedPurchaseReportForEachId>("up_GetTotalFeedPurchaseSingleReport @reportId", prmReportIdTotal).FirstOrDefault();
            FeedPurcaseTable sellingReport = DbContext.Set<FeedPurcaseTable>().Where(x => x.FeedSellingReportId == purchaseId).FirstOrDefault();
            pdfInfo.feedReportList = singleReportList;
            pdfInfo.totalReportEachItem = totalReport;
            pdfInfo.FeedPurcaseTable = sellingReport;
            return pdfInfo;
        }

        public IList<up_GetFeedPurchasePieChart> GetFeedPurchasePieChart()
        {
            List<up_GetFeedPurchasePieChart> result = DbContext.Database.SqlQuery<up_GetFeedPurchasePieChart>("up_GetFeedPurchasePieChart").ToList();
            return result;
        }

    }
}
