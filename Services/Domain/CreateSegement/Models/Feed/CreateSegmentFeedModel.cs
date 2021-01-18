using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Feed
{
    public class CreateSegmentFeedModel
    {
        public CreateSegmentFeed CreateSegmentFeed { get; set; }
        public bool isAddOperation { get; set; }
        public string LicenseStartDate { get; set; }

        public string LicenseEndDate { get; set; }

        public bool IsActive { get; set; }

        public bool CustomURL { get; set; }

        public bool DataUploadForHazardAndTrade { get; set; }
    }


    public class CreateFeedSegmentCategoriesModal
    {
        public CreateSegmentFeedCategories CreateSegmentFeedCategory { get; set; }
        public bool isAddOperation { get; set; }
       
    }
}