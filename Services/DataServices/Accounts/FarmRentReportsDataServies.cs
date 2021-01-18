using Core.DataService;
using Services.DataContext;
using Services.Domain.Accounts.Models.FarmRents;
using Services.Domain.CreateSegement.Models.Project;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Accounts
{
    public class FarmRentReportsDataServies : EntityContextDataService<FarmRentsCostsTable>
    {
        public FarmRentReportsDataServies(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public IList<CreateSegmentProject> GetAllProjectList()
        {
            List<CreateSegmentProject> projectList = DbContext.Database.SqlQuery<CreateSegmentProject>("SELECT * FROM Project where IsDeleted=0").ToList();

            return projectList;
        }


        public FarmRentsCostsTable GetFarmRentalDetails(int id)
        {
            FarmRentsCostsTable item = DbContext.Set<FarmRentsCostsTable>().Where(x => x.FarmRentalReportID == id).FirstOrDefault();

            return item;
        }


        public bool ProcessFarmRentReport(FarmRentsCostsTableDomain farmRentsCostsTableDomain, int adminUserId)
        {

            if (farmRentsCostsTableDomain != null && farmRentsCostsTableDomain.isAddOperation == true)
            {
                FarmRentsCostsTable item = new FarmRentsCostsTable();
                item.FarmRentalReportID = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportID;
                item.FarmRentalReportName = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportName;
                item.FarmRentalDate = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDate;
                //item.FarmRentalProjectID = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalProjectID;
                item.FarmRentalDetails = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDetails == null ? "": farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDetails;
                item.FarmRentalLandAmount = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalLandAmount;
                item.FarmRentalServieFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalServieFee;
                item.FarmRentalMainFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalMainFee;
                item.FarmRentalTotalFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalTotalFee;
                item.FarmRentalCostPerAmount = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalCostPerAmount;
                item.ProjectId = farmRentsCostsTableDomain.farmRentsCostsTable.ProjectId;
                item.AreaId = farmRentsCostsTableDomain.farmRentsCostsTable.AreaId;
                item.IsDeleted = false;

                item.CreatedDate = DateTime.Now;
                item.CreatedID = adminUserId;
                DbContext.Set<FarmRentsCostsTable>().Add(item);
                DbContext.SaveChanges();

                return true;
            }

            if (farmRentsCostsTableDomain != null && farmRentsCostsTableDomain.isAddOperation == false)
            {
                FarmRentsCostsTable item = DbContext.Set<FarmRentsCostsTable>().Where(x => x.FarmRentalReportID == farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportID).FirstOrDefault();

                item.FarmRentalReportName = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalReportName;
                item.FarmRentalDate = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDate;
                //item.FarmRentalProjectID = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalProjectID;
                item.FarmRentalDetails = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDetails == null ? "" : farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalDetails;
                item.FarmRentalLandAmount = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalLandAmount;
                item.FarmRentalServieFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalServieFee;
                item.FarmRentalMainFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalMainFee;
                item.FarmRentalTotalFee = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalTotalFee;
                item.FarmRentalCostPerAmount = farmRentsCostsTableDomain.farmRentsCostsTable.FarmRentalCostPerAmount;

                item.EditedDate = DateTime.Now;
                item.EditedID = adminUserId;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public IList<SPFarmRentalReportsBySearchParam> GetFarmRentalReportsBySearchParam( string projectId, string areaId)
        {
            
            SqlParameter prmProjectId = new SqlParameter("@projectId", projectId);
            SqlParameter prmAreaId = new SqlParameter("@areaId", areaId);

            IList<SPFarmRentalReportsBySearchParam> result = DbContext.Database.SqlQuery<SPFarmRentalReportsBySearchParam>("up_GetFarmRentalReportsBySearchParam @projectId,@areaId",  prmProjectId, prmAreaId).ToList();

            return result;
        }

        public int DeleteFarmRentalReport(int id)
        {
            int result = DbContext.Database.ExecuteSqlCommand("update FarmRentalReports set IsDeleted = 1 where FarmRentalReportID = " + id);
            return result;
        }

    }
}
