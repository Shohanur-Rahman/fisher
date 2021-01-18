using System.Linq;
using Services.DomainServices;
using Web.AppCode;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "User,Staff,SuperAdmin")]
    public class HomeController : BaseController
    {
        private readonly UserDomainService _userDomainService;

        #region Constructor

        public HomeController(UserDomainService userDomainServiceContext)
        {
            _userDomainService = userDomainServiceContext;
        }

        #endregion

        #region Page load related method


        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region  User Related Method

        public ActionResult About()
        {
            Services.Domain.User user = _userDomainService.GetAll().FirstOrDefault();

            if (user != null)
            {
               
                ViewData["Message"] = user.FirstName + " " + user.LastName;
            }

            return View();
        }

        #endregion

        #region Contact Related Method

        public ActionResult Contact()
        {
            return View();
        }

        #endregion

        #region Error message page

        //404 page not found
        public ActionResult PageNotFound()
        {
            return View("~/Views/ErrorPage/PageNotFound.cshtml");
        }

        //401 unauthorize custom error page

        public ActionResult UnAuthorizeError()
        {
            return View("~/Views/ErrorPage/UnAuthorizeError.cshtml");
        }
        //500 internal server error
        public ActionResult InternalServerError()
        {
            return View("~/Views/ErrorPage/InternalServerError.cshtml");
        }

      

        public ActionResult RedirectToDashboard()
        {
            string userRole = LoggedInUserInfoFromCookie.UserRoleInCookie;

            if(userRole== "SuperAdmin")
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            if(userRole == "User")
            {
                return RedirectToAction("Index", "UserDashboard");
            }

            return View(Index());
        }

        #endregion


    }
}
