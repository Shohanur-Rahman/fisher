using Kendo.Mvc.UI;
using Services.ApplicationEntity.TrashData;
using Services.ApplicationServices.TrashData;
using Services.Domain.TrashData;
using Services.DomainServices.TrashData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.TrashFile
{
    public class TrashDataController : BaseController
    {
        // GET: TrashData

        private readonly TrashDataDomainServices _trashDataDomainServices;

        protected TrashDataAppEntity _trashDataAppEntity;
        protected TrashDataApplicationServices _trashDataApplicationServices;

        public TrashDataController(TrashDataDomainServices trashDataDomainServices)
        {
            _trashDataDomainServices = trashDataDomainServices;
            _trashDataAppEntity = new TrashDataAppEntity();
            _trashDataAppEntity.trashDataDomainServices = trashDataDomainServices;
            _trashDataApplicationServices = new TrashDataApplicationServices(_trashDataAppEntity);

        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllTrashDataSearchResult(int tableID, int isAllData)
        {
            int totalRows = 0;
            
            IList<TrashDataList> dataResult = _trashDataApplicationServices.GetAllTrashDataSearchResult(tableID,isAllData);

            totalRows = dataResult.Count;

            var result = new DataSourceResult()
            {
                Data = dataResult,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RestoreTrashDataById(int tableNameId, int tableColumnId)
        {
            int result = _trashDataApplicationServices.RestoreTrashDataById(tableNameId, tableColumnId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}