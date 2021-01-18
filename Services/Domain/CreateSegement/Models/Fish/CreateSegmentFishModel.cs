using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Fish
{
   public class CreateSegmentFishModel
    {
        public CreateSegmentFish CreateSegmentFish { get; set; }
        public bool isAddOperation { get; set; }
    }



    public class CreateSegmentFishSellerModel
    {
        public CreateSegmentFishSeller CreateSegmentFishSeller { get; set; }
        public bool isAddOperation { get; set; }
    }



    [Table("FishSeller")]
    public class CreateSegmentFishSeller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Fish Name")]
        public string Name { get; set; }

        public int Age { get; set; }

        public bool IsDeleted { get; set; }
        public string Description { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public int? CreatedId { get; set; }

        public DateTime? CreatedDate { get; set; }

    }


    public class SPSegmentFishSellerList
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string CreaetdDate { get; set; }

        public int? CreatedId { get; set; }
    }

}
