using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Helper
{
    public class DateFormatHelper
    {

        /// <summary>
        /// Return Formatted Date Like: 01 Jun 17
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateWithFormat_DDMMMYY(DateTime date)
        {
            string formattedDate = date.Date.ToString("dd") + " " + date.Date.ToString("MMM") + " " + date.Date.ToString("yy");
            return formattedDate;

        }
    }
}