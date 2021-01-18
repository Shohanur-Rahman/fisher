using System;
using Core.Exceptions;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services.EmailService;
using System.Web.ModelBinding;

namespace Web.AppCode
{
    public class BaseController : Controller
    {
        protected virtual ActionResult DataActionViewService(Action dataAction, Func<ActionResult> dataActionComplete,
            Func<ActionResult> dataActionErrorAction)
        {
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    dataAction();

                }
                catch (CoreException ex)
                {

                    Emailer.SendMail("info@nobintraders.com", "Error From Core Exception Block Base Controller", ex.ErrorCode + "<br /> " + ex.Message);

                    ModelState.AddModelError(String.Empty, ExceptionMapper.MapException(ex).ViewMessage);
                    //MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, ex.Message);
                    return dataActionErrorAction();
                }
                catch (Exception ex)
                { 

                    Emailer.SendMail("info@nobintraders.com", "Error From Exception Block Base Controller", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "MessageConstants.ViewServiceDefaultErrorMessage");
                List<System.Web.Mvc.ModelState> errorList = ModelState.Values.ToList();
                string errorMessage = "";
                foreach (System.Web.Mvc.ModelState err in errorList)
                {
                    errorMessage += err.Value;
                }

                if (String.IsNullOrEmpty(errorMessage) == false)
                    Emailer.SendMail("info@nobintraders.com", "Error from IsValid False, Base Controller", errorMessage);

                //MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, errorMessage);
                return dataActionErrorAction();

            }

            return dataActionComplete();
        }

    }
}