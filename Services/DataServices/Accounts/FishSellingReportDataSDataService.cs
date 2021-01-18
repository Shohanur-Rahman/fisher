using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.PDF;
using Services.Domain.UserBilling;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Accounts
{
    public class FishSellingReportDataSDataService : EntityContextDataService<FishSellingReportTableDomain>
    {
        public FishSellingReportDataSDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }
        

        public IList<CreateSegmentFish> GetAllFishList()
        {
            List<CreateSegmentFish> segmentAreaList = DbContext.Database.SqlQuery<CreateSegmentFish>("SELECT * FROM Fish").ToList();

            return segmentAreaList;
        }

        public IList<CreateSegmentFishSeller> GetAllFishSellerNameList()
        {
            List<CreateSegmentFishSeller> result = DbContext.Database.SqlQuery<CreateSegmentFishSeller>("SELECT * FROM FishSeller where IsDeleted=0").ToList();

            return result;
        }


        public IList<SPFishSellingReportListForAdminBySearchParam> GetFishSellingReportForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);

            List<SPFishSellingReportListForAdminBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportListForAdminBySearchParam>("up_GetFishSellingReportForAdminByParam @startDate,@endDate,@isPartial,@areaId, @projectId", prmStartDate, prmEndDate, prmIsPartial, prmAreaId, prmprojectId).ToList();

            return fishSellingReportList;
        }
        public SPFishSellingReportGetTotalForAdminRecord GetFishSellingReportTotalForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);

            SPFishSellingReportGetTotalForAdminRecord fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportGetTotalForAdminRecord>("up_GetTotalFishSellingReportForAdminByParam @startDate,@endDate,@isPartial,@areaId, @projectId", prmStartDate, prmEndDate, prmIsPartial, prmAreaId, prmprojectId).FirstOrDefault();

            return fishSellingReportList;
        }

        public IList<SPFishSellingSellerReportForAdminByParam> GetFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);
            SqlParameter prmSellerId = new SqlParameter("@sellerID", sellerId);

            List<SPFishSellingSellerReportForAdminByParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingSellerReportForAdminByParam>("up_GetFishSellingSellerReportForAdminByParam @startDate,@endDate,@isPartial,@projectId,@sellerID", prmStartDate, prmEndDate, prmIsPartial, prmprojectId, prmSellerId).ToList();

            return fishSellingReportList;
        }

        public SPFishSellingReportGetTotalForAdminRecord GetTotalFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);
            SqlParameter prmSellerId = new SqlParameter("@sellerID", sellerId);

            SPFishSellingReportGetTotalForAdminRecord fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportGetTotalForAdminRecord>("up_GetTotalFishSellingSellerReportForAdminByParam @startDate,@endDate,@isPartial,@projectId,@sellerID", prmStartDate, prmEndDate, prmIsPartial, prmprojectId, prmSellerId).FirstOrDefault();

            return fishSellingReportList;
        }
        public IList<SPFishSellingReportListBySearchParam> GetFishSellingReportByParams(string areaId, string projectId, string sellerId, string isPartial)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectId);
            SqlParameter prmCalCulationName = new SqlParameter("@calCulationName", sellerId);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);  
           
            List<SPFishSellingReportListBySearchParam> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportListBySearchParam>("up_GetFishSellingReportByParam @areaId,@projectId,@calCulationName,@isPartial", prmAreaId, prmprojectId,prmCalCulationName, prmIsPartial).ToList();

            return fishSellingReportList;
        }


        public IList<SPFishSellingReportGetTotalRecord> GetTotalFishSellingCalculationReport(string areaId, string projectId, string calCulationName, string isPartial)
        {

            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectId);
            SqlParameter prmCalCulationName = new SqlParameter("@calCulationName", calCulationName);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);

            List<SPFishSellingReportGetTotalRecord> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportGetTotalRecord>("up_GetTotalFishSellingReportByParam @areaId,@projectId,@calCulationName,@isPartial", prmAreaId, prmprojectId, prmCalCulationName, prmIsPartial).ToList();

            return fishSellingReportList;
        }

        public int DeleteFishSellingRecord(int id)
        {
            int result = 0;

            int dueReportCount = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FishAmountDueReportMapper] where FishSellReportId = " + id + ";");
            int fishMapperCount = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FishSellingReportMapper] where FishSellReportId = " + id + ";");
            int fishRecord = DbContext.Database.ExecuteSqlCommand("DELETE FROM [FishSellingReport] where FishSellId = " + id);

            result = (dueReportCount + fishMapperCount + fishRecord);

            return result;
        }


        private int CheckFishIdExistInCurrentList(string thisFishId, string allFishId) {

            int returnValue = 0;

            if (allFishId != "")
            {
                var splitIdArray = allFishId.Split(',');

                for (int item = 0; item < splitIdArray.Length; item++)
                {
                    if (splitIdArray[item] == thisFishId)
                    {
                        returnValue = item + 1;
                    }
                }
            }

            return returnValue;
        }


        public FishSellingReportTableDataModal GetFishSellingDataReports(int id)
        {
            FishSellingReportTableDataModal sellingReport = new FishSellingReportTableDataModal();
            FishSellingReportTableDomain sellingOverview = new FishSellingReportTableDomain();
            List<FishSellingReportMapperTable> fishDetailsMapperList = new List<FishSellingReportMapperTable>();
            List<CreateSegmentFish> fishNameList = new List<CreateSegmentFish>();

            sellingOverview = DbContext.Set<FishSellingReportTableDomain>().Where(x => x.FishSellId == id).FirstOrDefault();
            fishDetailsMapperList = DbContext.Set<FishSellingReportMapperTable>().Where(x => x.FishSellReportId == id).ToList();
            fishNameList = DbContext.Set<CreateSegmentFish>().Where(x => x.ID != 0).ToList();

            sellingReport.FishSellingReportTableDomain = sellingOverview;
          


            string fishIdList = "";
            string existingMapperID = "";

            if (fishDetailsMapperList.Count > 0)
            {
                for (int item = 0; item < fishDetailsMapperList.Count; item++)
                {
                    if (fishIdList == "")
                    {
                        fishIdList = fishDetailsMapperList[item].SellFishId.ToString();
                    }
                    else
                    {
                        fishIdList = fishIdList + "," + fishDetailsMapperList[item].SellFishId.ToString();
                    }

                    if (existingMapperID == "")
                    {
                        existingMapperID = fishDetailsMapperList[item].FishSellMapperId.ToString();
                    }
                    else
                    {
                        existingMapperID = existingMapperID + "," + fishDetailsMapperList[item].FishSellMapperId.ToString();
                    }
                }
            }
            List<FishSellingReportMapperTable> fishItems = new List<FishSellingReportMapperTable>();

            for (int item = 0; item < fishNameList.Count; item++) {
                 
                var indexNumber = CheckFishIdExistInCurrentList(fishNameList[item].ID.ToString(), fishIdList);
                FishSellingReportMapperTable singleFish = new FishSellingReportMapperTable();

                if (indexNumber >= 1)
                {
                    fishItems.Add(fishDetailsMapperList[indexNumber - 1]);
                }
                else {
                    //Assign Value in New Fish Object
                    singleFish.FishSellMapperId = 0;
                    singleFish.FishSellReportId = 0;
                    singleFish.FishSellerId = 0;
                    singleFish.SellFishId = 0;
                    singleFish.TotalFishkg = 0;
                    singleFish.PiecesPerKG = 0;
                    singleFish.TotalPiecesFish = 0;
                    singleFish.PricePerKG = 0;
                    singleFish.TotalSellPrice = 0;

                    fishItems.Add(singleFish);
                }
                
            }

            sellingReport.FishSellingMapperList = fishItems;
            //FishSellingReportTableDomain tempFishOverViewList = new FishSellingReportTableDomain();

            sellingReport.FishIdListWithComma = fishIdList;
            sellingReport.ExistingMapperIdListForEdit = existingMapperID;

            return sellingReport;
        }


        public bool ProcessFishSellingCalculation(FishSellingReportTableDataModal fishSellingDataModal, int adminUserId)
        {
            
            try 
            {
               
                if (fishSellingDataModal != null && fishSellingDataModal.isAddOperation == true)
                {



                    FishSellingReportTableDomain accountsSellsFishSellCalculation = new FishSellingReportTableDomain();

                    accountsSellsFishSellCalculation.FishSellId = fishSellingDataModal.FishSellingReportTableDomain.FishSellId;
                    accountsSellsFishSellCalculation.HisabName = fishSellingDataModal.FishSellingReportTableDomain.HisabName;
                    accountsSellsFishSellCalculation.GarirName = fishSellingDataModal.FishSellingReportTableDomain.GarirName;
                    accountsSellsFishSellCalculation.TotalSellInKG = fishSellingDataModal.FishSellingReportTableDomain.TotalSellInKG;
                    accountsSellsFishSellCalculation.SellDate = fishSellingDataModal.FishSellingReportTableDomain.SellDate;
                    accountsSellsFishSellCalculation.FishSellerId = fishSellingDataModal.FishSellingReportTableDomain.FishSellerId;
                    accountsSellsFishSellCalculation.TotalSellPrice = fishSellingDataModal.FishSellingReportTableDomain.TotalSellPrice;
                    accountsSellsFishSellCalculation.SellNote = fishSellingDataModal.FishSellingReportTableDomain.SellNote;
                    accountsSellsFishSellCalculation.AreaId = fishSellingDataModal.FishSellingReportTableDomain.AreaId;
                    accountsSellsFishSellCalculation.ProjectId = fishSellingDataModal.FishSellingReportTableDomain.ProjectId;
                    accountsSellsFishSellCalculation.FishAmountPaid = fishSellingDataModal.FishSellingReportTableDomain.FishAmountPaid;
                    accountsSellsFishSellCalculation.FishAmountDue = fishSellingDataModal.FishSellingReportTableDomain.FishAmountDue;
                    accountsSellsFishSellCalculation.IsClosedByAdmin = fishSellingDataModal.FishSellingReportTableDomain.IsClosedByAdmin;

                    accountsSellsFishSellCalculation.CalculatedDate = DateTime.Now;
                    accountsSellsFishSellCalculation.CalculatedById = adminUserId;

                    DbContext.Set<FishSellingReportTableDomain>().Add(accountsSellsFishSellCalculation);
                    DbContext.SaveChanges();


                    //Insert Billing History
                    FishBillingHistoryTable billingHistory = new FishBillingHistoryTable();
                    billingHistory.FishBillingId = 1;
                    billingHistory.FishBillingFishSellId = accountsSellsFishSellCalculation.FishSellId;
                    billingHistory.FishBillingFishInKG = accountsSellsFishSellCalculation.TotalSellInKG;
                    billingHistory.FishBillingTotalPrice = accountsSellsFishSellCalculation.TotalSellPrice;
                    billingHistory.FishBillingPaidAmount = accountsSellsFishSellCalculation.FishAmountPaid;
                    billingHistory.FishBillingDueAmount = accountsSellsFishSellCalculation.FishAmountDue;
                    billingHistory.FishBillingUserId = adminUserId;
                    billingHistory.FishBillingDate = DateTime.Now;
                    billingHistory.BillingType = "I";
                    DbContext.Set<FishBillingHistoryTable>().Add(billingHistory);
                    DbContext.SaveChanges();

                    FishAmountDueReportMapperTable dueCalculation = new FishAmountDueReportMapperTable();
                    dueCalculation.FishAmountDue = accountsSellsFishSellCalculation.FishAmountDue;
                    dueCalculation.FishSellReportId = accountsSellsFishSellCalculation.FishSellId;
                    dueCalculation.FishSellerId = accountsSellsFishSellCalculation.FishSellerId;
                    dueCalculation.FishSellerReportMapperId = 0;

                    DbContext.Set<FishAmountDueReportMapperTable>().Add(dueCalculation);
                    DbContext.SaveChanges();

                    if (fishSellingDataModal.FishSellingMapperList != null && fishSellingDataModal.FishSellingMapperList.Count > 0)
                    {
                        for (int item = 0; item < fishSellingDataModal.FishSellingMapperList.Count; item++)
                        {
                            FishSellingReportMapperTable singleFish = new FishSellingReportMapperTable();
                            var thisItem = fishSellingDataModal.FishSellingMapperList[item];

                            if (thisItem.PricePerKG > 0 || thisItem.TotalFishkg > 0 || thisItem.TotalSellPrice > 0)
                            {
                                //Assign Value in New Fish Object
                                singleFish.FishSellMapperId = 0;
                                singleFish.FishSellReportId = accountsSellsFishSellCalculation.FishSellId;
                                singleFish.FishSellerId = accountsSellsFishSellCalculation.FishSellerId;
                                singleFish.SellFishId = thisItem.SellFishId;
                                singleFish.TotalFishkg = thisItem.TotalFishkg;
                                singleFish.PiecesPerKG = thisItem.PiecesPerKG;
                                singleFish.TotalPiecesFish = thisItem.TotalPiecesFish;
                                singleFish.PricePerKG = thisItem.PricePerKG;
                                singleFish.TotalSellPrice = thisItem.TotalSellPrice;

                                //Add Fish Item In Temp Fish List
                                
                                DbContext.Set<FishSellingReportMapperTable>().Add(singleFish);
                                DbContext.SaveChanges();
                            }

                        }

                    }

                    return true;
                }

                if (fishSellingDataModal != null && fishSellingDataModal.isAddOperation == false)
                {
                    FishSellingReportTableDomain currentRow = DbContext.Set<FishSellingReportTableDomain>().Where(x => x.FishSellId == fishSellingDataModal.FishSellingReportTableDomain.FishSellId).FirstOrDefault();

                    FishAmountDueReportMapperTable dueAmountRow = DbContext.Set<FishAmountDueReportMapperTable>().Where(x => x.FishSellReportId == fishSellingDataModal.FishSellingReportTableDomain.FishSellId).FirstOrDefault();


                    currentRow.FishSellId = fishSellingDataModal.FishSellingReportTableDomain.FishSellId;
                    currentRow.HisabName = fishSellingDataModal.FishSellingReportTableDomain.HisabName;
                    currentRow.GarirName = fishSellingDataModal.FishSellingReportTableDomain.GarirName;
                    currentRow.TotalSellInKG = fishSellingDataModal.FishSellingReportTableDomain.TotalSellInKG;
                    currentRow.SellDate = fishSellingDataModal.FishSellingReportTableDomain.SellDate;
                    currentRow.FishSellerId = fishSellingDataModal.FishSellingReportTableDomain.FishSellerId;
                    currentRow.TotalSellPrice = fishSellingDataModal.FishSellingReportTableDomain.TotalSellPrice;
                    currentRow.SellNote = fishSellingDataModal.FishSellingReportTableDomain.SellNote;
                    currentRow.AreaId = fishSellingDataModal.FishSellingReportTableDomain.AreaId;
                    currentRow.ProjectId = fishSellingDataModal.FishSellingReportTableDomain.ProjectId;
                    currentRow.FishAmountPaid = fishSellingDataModal.FishSellingReportTableDomain.FishAmountPaid;
                    currentRow.FishAmountDue = fishSellingDataModal.FishSellingReportTableDomain.FishAmountDue;

                    currentRow.CalculationEditedDate = DateTime.Now;
                    currentRow.CalCulationEditedId = adminUserId;


                    //Update Due Amount
                    dueAmountRow.FishSellerId= fishSellingDataModal.FishSellingReportTableDomain.FishSellerId;
                    dueAmountRow.FishAmountDue= fishSellingDataModal.FishSellingReportTableDomain.FishAmountDue;

                    var splitIdArray = fishSellingDataModal.ExistingMapperIdListForEdit.Split(',');
                    if (splitIdArray.Length > 0)
                    {
                        for (int item = 0; item < splitIdArray.Length; item++)
                        {
                            var mapId = splitIdArray[item];
                            int result = DbContext.Database.ExecuteSqlCommand("DELETE FROM[FishSellingReportMapper] where FishSellMapperId = " + mapId + ";");
                        }
                    }


                    if (fishSellingDataModal.FishSellingMapperList != null && fishSellingDataModal.FishSellingMapperList.Count > 0)
                    {
                        for (int item = 0; item < fishSellingDataModal.FishSellingMapperList.Count; item++)
                        {
                            FishSellingReportMapperTable singleFish = new FishSellingReportMapperTable();
                            var thisItem = fishSellingDataModal.FishSellingMapperList[item];

                            if (thisItem.PricePerKG > 0 || thisItem.TotalFishkg > 0 || thisItem.TotalSellPrice > 0)
                            {
                                //Assign Value in New Fish Object
                                singleFish.FishSellMapperId = 0;
                                singleFish.FishSellReportId = currentRow.FishSellId;
                                singleFish.FishSellerId = currentRow.FishSellerId;
                                singleFish.SellFishId = thisItem.SellFishId;
                                singleFish.TotalFishkg = thisItem.TotalFishkg;
                                singleFish.PiecesPerKG = thisItem.PiecesPerKG;
                                singleFish.TotalPiecesFish = thisItem.TotalPiecesFish;
                                singleFish.PricePerKG = thisItem.PricePerKG;
                                singleFish.TotalSellPrice = thisItem.TotalSellPrice;

                                //Add Fish Item In Temp Fish List

                                DbContext.Set<FishSellingReportMapperTable>().Add(singleFish);
                                DbContext.SaveChanges();
                            }

                        }

                    }

                    DbContext.SaveChanges();

                    FishBillingHistoryTable billingHistory = DbContext.Set<FishBillingHistoryTable>().Where(x => x.FishBillingFishSellId == fishSellingDataModal.FishSellingReportTableDomain.FishSellId).FirstOrDefault();
                    //Update Billing History
                    billingHistory.FishBillingFishSellId = currentRow.FishSellId;
                    billingHistory.FishBillingFishInKG = currentRow.TotalSellInKG;
                    billingHistory.FishBillingTotalPrice = currentRow.TotalSellPrice;
                    billingHistory.FishBillingPaidAmount = currentRow.FishAmountPaid;
                    billingHistory.FishBillingDueAmount = currentRow.FishAmountDue;
                    billingHistory.FishBillingUserId = adminUserId;
                    billingHistory.FishBillingDate = DateTime.Now;
                    billingHistory.BillingType = "U";

                    DbContext.SaveChanges();


                    

                    return true;
                }

                return false;
            }
            catch (Exception e) {
                throw e;
            }

        }


        public IList<SPFishSellingUserDashboardOverview> GetFishSellingDashboardOverview(int areaId, int projectID)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);

            List<SPFishSellingUserDashboardOverview> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingUserDashboardOverview>("up_GetFishSellingChartReportForUser @areaId,@projectId", prmAreaId, prmprojectId).ToList();

            return fishSellingReportList;
        }


        public IList<SPFishSellingReportGroupByFishName> GetFishSellingReportGroupByFishName(int areaId, int projectID)
        {
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);

            List<SPFishSellingReportGroupByFishName> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingReportGroupByFishName>("up_GetFishSellingReportByFishId @areaId,@projectId", prmAreaId, prmprojectId).ToList();

            return fishSellingReportList;
        }

        public IList<SPFishSellingUserDashboardOverview> GetFishSellingChartReportForAdmin()
        {
            List<SPFishSellingUserDashboardOverview> fishSellingReportList = DbContext.Database.SqlQuery<SPFishSellingUserDashboardOverview>("up_GetFishSellingColumnChartReportForAdmin").ToList();

            return fishSellingReportList;
        }

        public FishSellPDFInforForUser GetFishSellingPDFInformation(int sellId)
        {
            SqlParameter prmSellId = new SqlParameter("@sellId", sellId);
            SqlParameter prmSellIdInfo = new SqlParameter("@sellId", sellId);
            FishSellPDFInforForUser PDFInfo = new FishSellPDFInforForUser();
            IList<SingleFishSellReportList> reportList = DbContext.Database.SqlQuery<SingleFishSellReportList>("up_GetSingleFishReportOverview @sellId", prmSellId).ToList();
            FishSellPDFInfoForUser sellInfo = DbContext.Database.SqlQuery<FishSellPDFInfoForUser>("up_GetSingleFishReportForPDF @sellId", prmSellIdInfo).FirstOrDefault();
            PDFInfo.SingleFishSellReprot = reportList;
            PDFInfo.FishInfo = sellInfo; 
            return PDFInfo;
        }


        #region"Fish Seller PDF"

        public FishSellerInfoPDF GetFishSellerPDFInfo(int fishSellId)
        {
            SqlParameter prmFishSellId = new SqlParameter("@fishSellId", fishSellId);

            SqlParameter prmFishSellIdTotal = new SqlParameter("@fishSellId", fishSellId);

            FishSellerInfoPDF pdfInfo = new FishSellerInfoPDF();
             
            pdfInfo.Sell = DbContext.Database.SqlQuery<SellInfo>("up_GetFishSellerPDFInfo @fishSellId", prmFishSellId).ToList();
            pdfInfo.TotalSell = DbContext.Database.SqlQuery<SellInfoTotal>("up_GetFishSellerPDFInfoTotalCalculation @fishSellId", prmFishSellIdTotal).FirstOrDefault();
            pdfInfo.fishReport = DbContext.Set<FishSellingReportTableDomain>().Where(x => x.FishSellId == fishSellId).FirstOrDefault();

            return pdfInfo;
        }


        public FishSellingReportPDFInfo GetFishSellingReportPDFDataForAdmin(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate); 
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectId = new SqlParameter("@projectId", projectID);


            SqlParameter prmStartDateTotal = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDateTotal = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartialTotal = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmAreaIdTotal = new SqlParameter("@areaId", areaId);
            SqlParameter prmprojectIdTotal = new SqlParameter("@projectId", projectID);

            FishSellingReportPDFInfo pdfInfo = new FishSellingReportPDFInfo();

            pdfInfo.Sell = DbContext.Database.SqlQuery<SPFishSellingReportListForAdminBySearchParam>("up_GetFishSellingReportForAdminByParam @startDate,@endDate,@isPartial,@areaId, @projectId", prmStartDate, prmEndDate, prmIsPartial, prmAreaId, prmprojectId).ToList();
            pdfInfo.TotalSell = DbContext.Database.SqlQuery<SPFishSellingReportGetTotalForAdminRecord>("up_GetTotalFishSellingReportForAdminByParam @startDate,@endDate,@isPartial,@areaId, @projectId", prmStartDateTotal, prmEndDateTotal, prmIsPartialTotal, prmAreaIdTotal, prmprojectIdTotal).FirstOrDefault();


            return pdfInfo;
        }

        #endregion


    }
}
