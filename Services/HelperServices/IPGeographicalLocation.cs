using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.HelperServices
{
    public class IPGeographicalLocation
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("country_code")]

        public string CountryCode { get; set; }

        [JsonProperty("country_name")]

        public string CountryName { get; set; }

        [JsonProperty("region_code")]

        public string RegionCode { get; set; }

        [JsonProperty("region_name")]

        public string RegionName { get; set; }

        [JsonProperty("city")]

        public string City { get; set; }

        [JsonProperty("zip_code")]

        public string ZipCode { get; set; }

        [JsonProperty("time_zone")]

        public string TimeZone { get; set; }

        [JsonProperty("latitude")]

        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]

        public decimal Longitude { get; set; }

        [JsonProperty("metro_code")]

        public int MetroCode { get; set; }

    }
}