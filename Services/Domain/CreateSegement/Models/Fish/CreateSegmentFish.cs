using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Fish
{
   [Table("Fish")]
    public class CreateSegmentFish
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
}
