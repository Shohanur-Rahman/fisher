using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Feed
{

    [Table("Feed")]
    public class CreateSegmentFeed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Fish Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Display(Name = "Fish Details")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public int? CreatedId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }


    [Table("FeedCategories")]
    public class CreateSegmentFeedCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedCategoryId { get; set; }

        [StringLength(250)]
        [Display(Name = "Name")]
        public string FeedCategoryName { get; set; }

        [StringLength(500)]
        public string FeedCategoryImageUrl { get; set; }

        [Display(Name = "Fish Details")]
        public string FeedCategoryDetails { get; set; }

        public int FeedCategoryFeedId { get; set; }

        public int? CreatedId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }

}
