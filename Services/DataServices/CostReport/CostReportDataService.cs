using Core.DataService;
using Services.DataContext;
using Services.Domain.CostReport;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.CostReport
{
    public class CostReportDataService : EntityContextDataService<CostExpandTable>
    {
        public CostReportDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public bool ProcessCostName(CostReportDomains cCostReportDomains, int adminUserId)
        {


            if (cCostReportDomains != null && cCostReportDomains.isAddOperation == true)
            {
                CostExpandTable item = new CostExpandTable();
                item.ID = cCostReportDomains.costExpandTable.ID;
                item.Name = cCostReportDomains.costExpandTable.Name;

                if (cCostReportDomains.costExpandTable.ImageUrl != null)
                {
                    item.ImageUrl = cCostReportDomains.costExpandTable.ImageUrl;
                }
                else
                {
                    item.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                item.IsDeleted = false;
                item.Description = cCostReportDomains.costExpandTable.Description==null?"": cCostReportDomains.costExpandTable.Description;
                item.CreatedDate = DateTime.Now;
                item.CreatedId = adminUserId;
                DbContext.Set<CostExpandTable>().Add(item);
                DbContext.SaveChanges();

                return true;
            }

            if (cCostReportDomains != null && cCostReportDomains.isAddOperation == false)
            {
                CostExpandTable item = DbContext.Set<CostExpandTable>().Where(x => x.ID == cCostReportDomains.costExpandTable.ID).FirstOrDefault();

                item.Name = cCostReportDomains.costExpandTable.Name;

                if (cCostReportDomains.costExpandTable.ImageUrl != null)
                {
                    item.ImageUrl = cCostReportDomains.costExpandTable.ImageUrl;
                }
                else
                {
                    item.ImageUrl = "assets/global/img/rams-logo.jpeg";
                }

                item.Description = cCostReportDomains.costExpandTable.Description == null ? "" : cCostReportDomains.costExpandTable.Description;

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }



        public IList<CostExpandNamePorperty> GetCostExpandNameList(string searchText)
        {
            string srchText = string.IsNullOrEmpty(searchText) == true ? "" : searchText.Trim();

            SqlParameter prmSearchText = new SqlParameter("@searchText", srchText);

            List<CostExpandNamePorperty> fishSellerList = DbContext.Database.SqlQuery<CostExpandNamePorperty>("up_GetCostExpandName @searchText", prmSearchText).ToList();

            return fishSellerList;
        }


        public CostExpandTable GetCostExpandNameDetails(int id)
        {
            CostExpandTable item = DbContext.Set<CostExpandTable>().Where(x => x.ID == id).FirstOrDefault();

            return item;
        }

        public int DeleteCostExpandName(int id)
        {

            int result = DbContext.Database.ExecuteSqlCommand("Update [Expend] set IsDeleted = 1 where id=" + id);

            return result;
        }




    }
}
