using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.SPModels
{
    public class SPCreateSegmentProjectList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Cost { get; set; }
        public string Time { get; set; }
        public string Land { get; set; }

        public string ImageUrl { get; set; }
        public string CreaetdDate { get; set; }
        public int TotalRows { get; set; }
    }
}
