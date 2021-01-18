using Core.DomainService;
using Services.DataServices.Accounts;
using Services.Domain.Accounts.Models.FarmRents;
using Services.Domain.CreateSegement.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.Accounts
{
    public class FarmRentsReportsDomainSerices : DomainService<FarmRentsCostsTable, long>
    {
        private readonly FarmRentReportsDataServies _farmRentReportsDataServies;
        public FarmRentsReportsDomainSerices(FarmRentReportsDataServies farmRentReportsDataServies) : base(farmRentReportsDataServies)
        {
            _farmRentReportsDataServies = farmRentReportsDataServies;
        }

        public IList<CreateSegmentProject> GetAllProjectList()
        {
            return _farmRentReportsDataServies.GetAllProjectList();
        }


        public FarmRentsCostsTable GetFarmRentalDetails(int id)
        {
            return _farmRentReportsDataServies.GetFarmRentalDetails(id);
        }

        public bool ProcessFarmRentReport(FarmRentsCostsTableDomain farmRentsCostsTableDomain, int adminUserId)
        {
            return _farmRentReportsDataServies.ProcessFarmRentReport(farmRentsCostsTableDomain, adminUserId);
        }

        public IList<SPFarmRentalReportsBySearchParam> GetFarmRentalReportsBySearchParam(string projectId, string areaId)
        {
            return _farmRentReportsDataServies.GetFarmRentalReportsBySearchParam( projectId, areaId);
        }


        public int DeleteFarmRentalReport(int id)
        {
            return _farmRentReportsDataServies.DeleteFarmRentalReport(id);
        }

    }
}
