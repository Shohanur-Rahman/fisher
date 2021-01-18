using Services.ApplicationEntity.Accounts;
using Services.Domain.Accounts.Models.FarmRents;
using Services.Domain.CreateSegement.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Accounts
{
    public class FarmRentReportsApplicationService
    {
        protected FarmRentReportsAppEntity _farmRentReportsAppEntityInstance { get; set; }

        public FarmRentReportsApplicationService(FarmRentReportsAppEntity farmRentReportsAppEntity)
        {
            _farmRentReportsAppEntityInstance = farmRentReportsAppEntity;
        }

        public IList<CreateSegmentProject> GetAllProjectList()
        {
            return _farmRentReportsAppEntityInstance.farmRentsReportsDomainSerices.GetAllProjectList();
        }

        public FarmRentsCostsTable GetFarmRentalDetails(int id)
        {
            return _farmRentReportsAppEntityInstance.farmRentsReportsDomainSerices.GetFarmRentalDetails(id);
        }


        public bool ProcessFarmRentReport(FarmRentsCostsTableDomain farmRentsCostsTableDomain, int adminUserId)
        {
            return _farmRentReportsAppEntityInstance.farmRentsReportsDomainSerices.ProcessFarmRentReport(farmRentsCostsTableDomain, adminUserId);
        }


        public IList<SPFarmRentalReportsBySearchParam> GetFarmRentalReportsBySearchParam(string projectId, string areaId)
        {
            return _farmRentReportsAppEntityInstance.farmRentsReportsDomainSerices.GetFarmRentalReportsBySearchParam( projectId, areaId);
        }


        public int DeleteFarmRentalReport(int id)
        {
            return _farmRentReportsAppEntityInstance.farmRentsReportsDomainSerices.DeleteFarmRentalReport(id);
        }

    }
}
