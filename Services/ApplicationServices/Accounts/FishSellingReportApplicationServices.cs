using Services.ApplicationEntity.Accounts;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Accounts
{
    public class FishSellingReportApplicationServices
    {
        protected FishSellingReportAppEntity _FishSellingReportAppEntityAppEntityInstance { get; set; }

        public FishSellingReportApplicationServices(FishSellingReportAppEntity fishSellingReportAppEntity)
        {
            _FishSellingReportAppEntityAppEntityInstance = fishSellingReportAppEntity;
        }
        
        public IList<CreateSegmentFish> GetAllFishList()
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetAllFishList();
        }

        public IList<CreateSegmentFishSeller> GetAllFishSellerNameList()
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetAllFishSellerNameList();
        }

        public IList<SPFishSellingReportListBySearchParam> GetFishSellingReportByParams(string areaId, string projectId, string sellerId, string isPartial)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingReportByParams(areaId, projectId, sellerId, isPartial);
        }

        public IList<SPFishSellingReportListForAdminBySearchParam> GetFishSellingReportForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingReportForAdminByParams(startDate, endDate, isPartial, areaId, projectID);
        }
        public SPFishSellingReportGetTotalForAdminRecord GetFishSellingReportTotalForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingReportTotalForAdminByParams(startDate, endDate, isPartial, areaId, projectID);
        }

        public IList<SPFishSellingSellerReportForAdminByParam> GetFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial,  projectID, sellerId);
        }
        public SPFishSellingReportGetTotalForAdminRecord GetTotalFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetTotalFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial, projectID, sellerId);
        }

        public IList<SPFishSellingReportGetTotalRecord> GetTotalFishSellingCalculationReport(string areaId, string projectId, string calCulationName, string isPartial)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetTotalFishSellingCalculationReport(areaId, projectId, calCulationName, isPartial);
        }

        public int DeleteFishSellingRecord(int id)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.DeleteFishSellingRecord(id);
        }

        public FishSellingReportTableDataModal GetFishSellingDataReports(int feedId)
        {
            FishSellingReportTableDataModal sellingReport = _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingDataReports(feedId);
            if (sellingReport != null)
            {
                return sellingReport; 

            }
            return null;
        }

        public bool ProcessFishSellingCalculation(FishSellingReportTableDataModal fishSellingReportTableDataModal, int adminUserId)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.ProcessFishSellingCalculation(fishSellingReportTableDataModal, adminUserId);
        }

        public IList<SPFishSellingUserDashboardOverview> GetFishSellingDashboardOverview(int areaId, int projectID)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingDashboardOverview(areaId, projectID);
        }

        public IList<SPFishSellingReportGroupByFishName> GetFishSellingReportGroupByFishName(int areaId, int projectID)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingReportGroupByFishName(areaId, projectID);
        }
        public IList<SPFishSellingUserDashboardOverview> GetFishSellingChartReportForAdmin()
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingChartReportForAdmin();
        }

        public FishSellPDFInforForUser GetFishSellingPDFInformation(int sellId)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingPDFInformation(sellId);
        }




        #region"Fish Seller PDF"

        public FishSellerInfoPDF GetFishSellerPDFInfo(int fishSellId)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellerPDFInfo(fishSellId);
        }

        public FishSellingReportPDFInfo GetFishSellingReportPDFDataForAdmin(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _FishSellingReportAppEntityAppEntityInstance.fishSellingDomainServices.GetFishSellingReportPDFDataForAdmin(startDate, endDate, isPartial, areaId, projectID);
        }

        #endregion

    }
}
