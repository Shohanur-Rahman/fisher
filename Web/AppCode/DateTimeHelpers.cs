using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AppCode
{
    public class DateTimeHelpers
    {
        public static DateTime ConvertUKDateStringToDateTimeObject(string dateStringUKFormat)
        {
            int day = int.Parse(dateStringUKFormat.Split('/')[0]);
            int month = int.Parse(dateStringUKFormat.Split('/')[1]);
            int year = int.Parse(dateStringUKFormat.Split('/')[2]);

            DateTime convertedDate = new DateTime(year,month,day);

            return convertedDate;
        }
    }
}
