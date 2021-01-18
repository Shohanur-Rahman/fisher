using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.UserBilling;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.UserBilling
{
    public class UserBillingDataService : EntityContextDataService<FishListForUserBilling>
    {
        public UserBillingDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public FishSellingDueBillingInfo GetFishSellingListForBilling(string isPartial, string sellerID)
        {
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmSellerId = new SqlParameter("@sellerID", sellerID);
            SqlParameter prmIsPartialTotal = new SqlParameter("@isPartial", isPartial);
            SqlParameter prmSellerIdTotal = new SqlParameter("@sellerID", sellerID);

            FishSellingDueBillingInfo BillingHistory = new FishSellingDueBillingInfo();

            List<FishListForUserBilling> singleReportList = DbContext.Database.SqlQuery<FishListForUserBilling>("up_GetFishSellingSellerReportForUserByParam @isPartial,@sellerID", prmIsPartial, prmSellerId).ToList();
            TotalDueBilling Total = DbContext.Database.SqlQuery<TotalDueBilling>("up_GetTotalFishSellingSellerReportForUserByParam @isPartial,@sellerID", prmIsPartialTotal, prmSellerIdTotal).FirstOrDefault();

            BillingHistory.FisnSellingReportList = singleReportList;
            BillingHistory.TotalBill = Total;

            return BillingHistory;
        }

        public BillingAmountHistoryBySellerId GetTotalAmountPaidAmountAndDueAmount(string fishSellId)
        {
            SqlParameter prmFishSellId = new SqlParameter("@fishSellId", fishSellId);
            SqlParameter prmFishSellIdList = new SqlParameter("@fishSellId", fishSellId);
            BillingAmountHistoryBySellerId Billing = new BillingAmountHistoryBySellerId();

            FishSellingDueAndTotalAmount singleReportList = DbContext.Database.SqlQuery<FishSellingDueAndTotalAmount>("up_GetFishSellingTotalAmountPaidAmountAndDueAmountBySellId @fishSellId", prmFishSellId).FirstOrDefault();
            IList<ListOfBillingAmountForSeller> ListOfBilling = DbContext.Database.SqlQuery<ListOfBillingAmountForSeller>("up_GetFishSellingBillingAmountPaidAmountAndDueAmountBySellId @fishSellId", prmFishSellIdList).ToList();
            Billing.BillingList = ListOfBilling;
            Billing.TotalBilling = singleReportList;

            return Billing;
        }

        public bool UpdateFishSellingBilling(string fishSellerId, string paidAmount, int adminUserId)
        {
            try
            {
                var returnResult = false;
                int sellerId = Convert.ToInt32(fishSellerId);
                decimal paidAmountDb = Convert.ToDecimal(paidAmount);

                List<FishSellingReportTableDomain> sellingReportList = DbContext.Set<FishSellingReportTableDomain>().Where(x => (x.FishSellerId == sellerId)).Where(x => (x.IsClosedByAdmin == 0)).ToList();

                int index = 0;
                for (index = 0; index < sellingReportList.Count; index++)
                {

                    if (paidAmountDb > 0)
                    {

                        var fishSellId = sellingReportList[index].FishSellId;

                        FishAmountDueReportMapperTable dueAmountRow = DbContext.Set<FishAmountDueReportMapperTable>().Where(x => x.FishSellReportId == fishSellId).FirstOrDefault();

                        FishSellingReportTableDomain currentRow = DbContext.Set<FishSellingReportTableDomain>().Where(x => x.FishSellId == fishSellId).FirstOrDefault();



                        decimal amountPaid = 0;
                        decimal amountDue = 0;
                        decimal billingAmount = 0;

                        if (currentRow.FishAmountDue >= paidAmountDb)
                        {
                            amountPaid = (currentRow.FishAmountPaid + paidAmountDb);
                            amountDue = (currentRow.TotalSellPrice - amountPaid);
                            billingAmount = paidAmountDb;
                            paidAmountDb = 0;
                        }
                        else
                        {

                            decimal dueAmount = currentRow.FishAmountDue;
                            amountPaid = (currentRow.FishAmountPaid + dueAmount);
                            amountDue = (currentRow.TotalSellPrice - amountPaid);
                            paidAmountDb = (paidAmountDb - dueAmount);
                            billingAmount = dueAmount;

                        }



                        currentRow.FishAmountPaid = amountPaid;
                        currentRow.FishAmountDue = amountDue;
                        if (currentRow.FishAmountPaid >= currentRow.TotalSellPrice)
                        {
                            currentRow.IsClosedByAdmin = 1;
                        }

                        // Update Due Amount
                        if (dueAmountRow != null)
                        {
                            dueAmountRow.FishAmountDue = amountDue;
                        }

                        DbContext.SaveChanges();


                        //Insert Billing History
                        FishBillingHistoryTable billingHistory = new FishBillingHistoryTable();
                        billingHistory.FishBillingId = 1;
                        billingHistory.FishBillingFishSellId = fishSellId;
                        billingHistory.FishBillingFishInKG = currentRow.TotalSellInKG;
                        billingHistory.FishBillingTotalPrice = currentRow.TotalSellPrice;
                        billingHistory.FishBillingPaidAmount = billingAmount;
                        billingHistory.FishBillingDueAmount = amountDue;
                        billingHistory.FishBillingUserId = adminUserId;
                        billingHistory.FishBillingDate = DateTime.Now;
                        billingHistory.BillingType = "B";
                        DbContext.Set<FishBillingHistoryTable>().Add(billingHistory);
                        DbContext.SaveChanges();

                    }
                    returnResult = true;
                }


                return returnResult;
            }
            catch (Exception e) {
                throw e;
            }
        }


        public IList<FishSellingBillingHistoryList> GetBillingFishSellingSearchResult(string sellerId, string startDate, string endDate, string isPartial)
        {
            SqlParameter prmSellerId = new SqlParameter("@sellerId", sellerId);
            SqlParameter prmStartDate = new SqlParameter("@startDate", startDate);
            SqlParameter prmEndDate = new SqlParameter("@endDate", endDate);
            SqlParameter prmIsPartial = new SqlParameter("@isPartial", isPartial);

            IList<FishSellingBillingHistoryList> billingRrportList = DbContext.Database.SqlQuery<FishSellingBillingHistoryList>("up_GetFishBillingHistoryBySearchParam @sellerId,@startDate,@endDate,@isPartial", prmSellerId,prmStartDate,prmEndDate,prmIsPartial).ToList();
            
            return billingRrportList;
        }


        public BillingAmountHistoryBySellerId GetFishSellingFullReportBySellerId(string fishSellId)
        {
            BillingAmountHistoryBySellerId Billing = new BillingAmountHistoryBySellerId();

            SqlParameter prmFishSellId = new SqlParameter("@fishSellId", fishSellId);
            SqlParameter prmFishSellIdList = new SqlParameter("@fishSellId", fishSellId);

            List<ListOfBillingAmountForSeller> sellingListWithMapper = new List<ListOfBillingAmountForSeller>();

            FishSellingDueAndTotalAmount totalBolling = DbContext.Database.SqlQuery<FishSellingDueAndTotalAmount>("up_GetFishSellingTotalAmountPaidAmountAndDueAmountBySellId @fishSellId", prmFishSellId).FirstOrDefault();
            IList<ListOfBillingAmountForSeller> listOfSell = DbContext.Database.SqlQuery<ListOfBillingAmountForSeller>("up_GetFishSellingBillingAmountPaidAmountAndDueAmountBySellId @fishSellId", prmFishSellIdList).ToList();

            for (int listIndex = 0; listIndex < listOfSell.Count; listIndex++) {
                int sellId = 0;
                if (listOfSell[listIndex].FishSellId > 0) {
                    sellId = listOfSell[listIndex].FishSellId;
                    SqlParameter prmSellId = new SqlParameter("@fishSellId", sellId);
                    IList<FishDueAmountListFishSellMapperList> mapperList = DbContext.Database.SqlQuery<FishDueAmountListFishSellMapperList>("up_GetFishSellingMapperListBySellId @fishSellId", prmSellId).ToList();
                    ListOfBillingAmountForSeller singleSellItem = new ListOfBillingAmountForSeller();
                    singleSellItem = listOfSell[listIndex];
                    singleSellItem.SellListMapperItems = mapperList;
                    sellingListWithMapper.Add(singleSellItem);
                }
            }

            Billing.BillingList = sellingListWithMapper;
            Billing.TotalBilling = totalBolling;

            return Billing;
        }


        public FeedBillingReport GetFeedBillingReportByFeedId(string feedId)
        {
            SqlParameter prmFeedId = new SqlParameter("@feedId", feedId);
            SqlParameter prmFeedIdTotal = new SqlParameter("@feedId", feedId);
            FeedBillingReport billingReport = new FeedBillingReport();
            IList<up_GetFeedBillingReportByFeedId> billingRrportList = DbContext.Database.SqlQuery<up_GetFeedBillingReportByFeedId>("up_GetFeedBillingReportByFeedId @feedId", prmFeedId).ToList();
            up_GetTotalFeedBillingReportByFeedId billingTotal = DbContext.Database.SqlQuery<up_GetTotalFeedBillingReportByFeedId>("up_GetTotalFeedBillingReportByFeedId @feedId", prmFeedIdTotal).FirstOrDefault();

            billingReport.BillingList = billingRrportList;
            billingReport.TotalBilling = billingTotal;

            return billingReport;
        }

        public FishBillingDownloadInfo GetFishSellingTotalBySellerId(int sellerId)
        {
            SqlParameter prmSellerId = new SqlParameter("@sellerId", sellerId);
            SqlParameter prmSellerIdTotal = new SqlParameter("@sellerId", sellerId);
            FishBillingDownloadInfo billing = new FishBillingDownloadInfo();
            IList<FishSellingTotalBySellerID> totalBolling = DbContext.Database.SqlQuery<FishSellingTotalBySellerID>("up_GetFishSellingTotalForSellerID @sellerId", prmSellerIdTotal).ToList();
            IList<FishBillingHistoryBySellerId> billingHistory = DbContext.Database.SqlQuery<FishBillingHistoryBySellerId>("up_GetFishBillingHistoryBySellerId @sellerId", prmSellerId).ToList();

            billing.BillingHistory = billingHistory;
            billing.TotalSellingInfo = totalBolling;

            return billing;
        }


    }
}
