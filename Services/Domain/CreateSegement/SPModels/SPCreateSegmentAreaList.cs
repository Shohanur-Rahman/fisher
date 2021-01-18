using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.SPModels
{
    public class SPCreateSegmentAreaList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Union_Name { get; set; }

        public string WardNumber { get; set; }

        public string ImageUrl { get; set; }
        public string CreaetdDate { get; set; }
        public int TotalRows { get; set; }
    }
}
