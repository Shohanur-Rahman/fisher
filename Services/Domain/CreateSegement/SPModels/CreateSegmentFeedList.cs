using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.SPModels
{
    public class CreateSegmentFeedList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CreaetdDate { get; set; }
        public int TotalRows { get; set; }
    }

    public class SPCreateSegmentFeedCategoryList
    {
        public int FeedCategoryId { get; set; }
        public string FeedCategoryName { get; set; }
        public string FeedCategoryDetails { get; set; }
        public string FeedCategoryImageUrl { get; set; }
        public string CreaetdDate { get; set; }
        public int TotalRows { get; set; }
    }

}
