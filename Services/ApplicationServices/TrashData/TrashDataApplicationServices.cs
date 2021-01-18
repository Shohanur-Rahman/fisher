using Services.ApplicationEntity.TrashData;
using Services.Domain.TrashData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.TrashData
{
    public class TrashDataApplicationServices
    {
        protected TrashDataAppEntity _trashDataAppEntityInstance { get; set; }

        public TrashDataApplicationServices(TrashDataAppEntity trashDataAppEntity)
        {
            _trashDataAppEntityInstance = trashDataAppEntity;
        }

        public IList<TrashDataList> GetAllTrashDataSearchResult(int tableID, int isAllData)
        {
            return _trashDataAppEntityInstance.trashDataDomainServices.GetAllTrashDataSearchResult(tableID,isAllData);
        }
        public int RestoreTrashDataById(int tableNameId, int tableColumnId)
        {
            return _trashDataAppEntityInstance.trashDataDomainServices.RestoreTrashDataById(tableNameId, tableColumnId);
        }
    }
}
