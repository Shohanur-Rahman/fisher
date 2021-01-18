using Core.DomainService;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.Accounts
{
   public class FishSellingDomainServices : DomainService<FishSellingReportTableDomain, long>
    {
        private readonly FishSellingReportDataSDataService _fishSellingReportDataSDataService;
        public FishSellingDomainServices(FishSellingReportDataSDataService fishSellingReportDataSDataService) : base(fishSellingReportDataSDataService)
        {
            _fishSellingReportDataSDataService = fishSellingReportDataSDataService;
        }
        
        public IList<CreateSegmentFish> GetAllFishList()
        {
            return _fishSellingReportDataSDataService.GetAllFishList();
        }

        public IList<CreateSegmentFishSeller> GetAllFishSellerNameList()
        {
            return _fishSellingReportDataSDataService.GetAllFishSellerNameList();
        }

        public IList<SPFishSellingReportListBySearchParam> GetFishSellingReportByParams(string areaId, string projectId, string sellerId, string isPartial)
        {
            return _fishSellingReportDataSDataService.GetFishSellingReportByParams(areaId, projectId, sellerId, isPartial);
        }

        public IList<SPFishSellingReportListForAdminBySearchParam> GetFishSellingReportForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _fishSellingReportDataSDataService.GetFishSellingReportForAdminByParams(startDate, endDate, isPartial, areaId, projectID);
        }
        public SPFishSellingReportGetTotalForAdminRecord GetFishSellingReportTotalForAdminByParams(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _fishSellingReportDataSDataService.GetFishSellingReportTotalForAdminByParams(startDate, endDate, isPartial, areaId, projectID);
        }
        public IList<SPFishSellingSellerReportForAdminByParam> GetFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            return _fishSellingReportDataSDataService.GetFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial,projectID, sellerId);
        }
        public SPFishSellingReportGetTotalForAdminRecord GetTotalFishSellingSellerReportForAdminByParams(string startDate, string endDate, string isPartial, string projectID, string sellerId)
        {
            return _fishSellingReportDataSDataService.GetTotalFishSellingSellerReportForAdminByParams(startDate, endDate, isPartial, projectID, sellerId);
        }
        public IList<SPFishSellingReportGetTotalRecord> GetTotalFishSellingCalculationReport(string areaId, string projectId, string calCulationName, string isPartial)
        {
            return _fishSellingReportDataSDataService.GetTotalFishSellingCalculationReport(areaId, projectId, calCulationName, isPartial);
        }


        public int DeleteFishSellingRecord(int id)
        {
            return _fishSellingReportDataSDataService.DeleteFishSellingRecord(id);
        }

        public FishSellingReportTableDataModal GetFishSellingDataReports(int feedId)
        {
            return _fishSellingReportDataSDataService.GetFishSellingDataReports(feedId);
        }

        public bool ProcessFishSellingCalculation(FishSellingReportTableDataModal fishSellingReportTableDataModal, int adminUserId)
        {
            return _fishSellingReportDataSDataService.ProcessFishSellingCalculation(fishSellingReportTableDataModal, adminUserId);
        }

        public IList<SPFishSellingUserDashboardOverview> GetFishSellingDashboardOverview(int areaId, int projectID)
        {
            return _fishSellingReportDataSDataService.GetFishSellingDashboardOverview(areaId, projectID);
        }

        public IList<SPFishSellingReportGroupByFishName> GetFishSellingReportGroupByFishName(int areaId, int projectID)
        {
            return _fishSellingReportDataSDataService.GetFishSellingReportGroupByFishName(areaId, projectID);
        }
        public IList<SPFishSellingUserDashboardOverview> GetFishSellingChartReportForAdmin()
        {
            return _fishSellingReportDataSDataService.GetFishSellingChartReportForAdmin();
        }

        public FishSellPDFInforForUser GetFishSellingPDFInformation(int sellId)
        {
            return _fishSellingReportDataSDataService.GetFishSellingPDFInformation(sellId);
        }

        #region"Fish Seller PDF"

        public FishSellerInfoPDF GetFishSellerPDFInfo(int fishSellId)
        {
            return _fishSellingReportDataSDataService.GetFishSellerPDFInfo(fishSellId);
        }

        public FishSellingReportPDFInfo GetFishSellingReportPDFDataForAdmin(string startDate, string endDate, string isPartial, string areaId, string projectID)
        {
            return _fishSellingReportDataSDataService.GetFishSellingReportPDFDataForAdmin(startDate, endDate, isPartial, areaId, projectID);
        }

        #endregion


    }
}
