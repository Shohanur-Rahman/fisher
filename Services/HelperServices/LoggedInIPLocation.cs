using System;

namespace Services.HelperServices
{
    public class LoggedInIPLocation
    {
        public string IP { get; set; }
        public string Country_Code { get; set; }
        public string Country_Name { get; set; }
        public string Region_Code { get; set; }
        public string Region_Name { get; set; }
        public string City { get; set; }
        public string Zip_Code { get; set; }
        public string Time_Zone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Metro_Code { get; set; }

    }
}