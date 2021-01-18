using Core.DataService;
using Services.DataContext;
using Services.Domain.TrashData;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.TrashData
{
    public class TrashDataDataServices : EntityContextDataService<TrashDataList>
    {
        public TrashDataDataServices(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

        public IList<TrashDataList> GetAllTrashDataSearchResult(int tableID, int isAllData)
        {

            SqlParameter prmTableId = new SqlParameter("@tableID", tableID);
            SqlParameter prmIsAllData = new SqlParameter("@isAllData", isAllData);

            List<TrashDataList> result = DbContext.Database.SqlQuery<TrashDataList>("up_GetTrashDataByTableID @tableID,@isAllData", prmTableId, prmIsAllData).ToList();

            return result;
        }

        public int RestoreTrashDataById(int tableNameId, int tableColumnId)
        {
            int result = 0;

            if (tableNameId == 1) {
                result = DbContext.Database.ExecuteSqlCommand("Update [Fish] set IsDeleted = 0 where id=" + tableColumnId);
            }
            if (tableNameId == 2)
            {
                result = DbContext.Database.ExecuteSqlCommand("Update [Feed] set IsDeleted = 0 where id=" + tableColumnId);
            }
            if (tableNameId == 3)
            {
                result = DbContext.Database.ExecuteSqlCommand("Update [FeedCategories] set IsDeleted = 0 where FeedCategoryId=" + tableColumnId);
            }
            if (tableNameId == 4)
            {
                result = DbContext.Database.ExecuteSqlCommand("Update [Area] set IsDeleted = 0 where id=" + tableColumnId);
            }
            if (tableNameId == 5)
            {
                result = DbContext.Database.ExecuteSqlCommand("Update [Project] set IsDeleted = 0 where id=" + tableColumnId);
            }
            if (tableNameId == 6)
            {
                result = DbContext.Database.ExecuteSqlCommand("Update [FishSeller] set IsDeleted = 0 where id=" + tableColumnId);
            }
            return result;
        }

    }
}
