using Services.ApplicationEntity.Accounts;
using Services.ApplicationServices.Accounts;
using Services.DomainServices.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers.Accounts
{
    public class MinnowController : BaseController
    {
        // GET: Minnow

        private readonly MinnowReportDomainServices _minnowReportDomainServices;

        protected MinnowReportAppEntity _minnowReportAppEntity;
        protected MinnowReportApplicationService _minnowReportApplicationService;

        public MinnowController(MinnowReportDomainServices minnowReportDomainServices)
        {
            try
            {
                _minnowReportDomainServices = minnowReportDomainServices;
                _minnowReportAppEntity = new MinnowReportAppEntity();
                _minnowReportAppEntity.minnowReportDomainServices = minnowReportDomainServices;
                _minnowReportApplicationService = new MinnowReportApplicationService(_minnowReportAppEntity);
            }
            catch (Exception e) {
                throw e;
            }

        }

        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/Accounts/Minnow/Index.cshtml");
        }
    }
}