using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.AppCode
{
    public class MessageDisplayHelper
    {
        public static string InformationMessageSetOrDisplay(Controller controller,bool isMsgSet,string message)
        {

            if (isMsgSet == true)
                controller.TempData["InfoMessage"] = message;
            else
                return controller.TempData["InfoMessage"] != null ? controller.TempData["InfoMessage"].ToString() : null;

            return null;
        }

        public static string ErrorMessageSetOrDisplay(Controller controller, bool isMsgSet, string message)
        {

            if (isMsgSet == true)
                controller.TempData["ErrorMessage"] = message;
            else
                return controller.TempData["ErrorMessage"] != null ? controller.TempData["ErrorMessage"].ToString() : null;

            return null;
        }


        public static string NotificationMessageSetOrDisplay(Controller controller, bool isMsgSet, string message)
        {

            if (isMsgSet == true)
                controller.TempData["NotificationMessage"] = message;
            else
                return controller.TempData["NotificationMessage"] != null ? controller.TempData["NotificationMessage"].ToString() : null;

            return null;
        }


    }//end class
}
