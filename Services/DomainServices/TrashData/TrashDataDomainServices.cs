using Core.DomainService;
using Services.DataServices.TrashData;
using Services.Domain.TrashData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainServices.TrashData
{
    public class TrashDataDomainServices : DomainService<TrashDataList, long>
    {
        private readonly TrashDataDataServices _trashDataDataServices;
        public TrashDataDomainServices(TrashDataDataServices trashDataDataServices) : base(trashDataDataServices)
        {
            _trashDataDataServices = trashDataDataServices;
        }


        public IList<TrashDataList> GetAllTrashDataSearchResult(int tableID, int isAllData)
        {
            return _trashDataDataServices.GetAllTrashDataSearchResult(tableID, isAllData);
        }

        public int RestoreTrashDataById(int tableNameId, int tableColumnId)
        {
            return _trashDataDataServices.RestoreTrashDataById(tableNameId, tableColumnId);
        }
    }
}
