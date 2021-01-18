using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Area
{
    [Table("Area")]
    public class CreateSegmentArea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Area Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Union")]
        public string Union_Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Ward Number")]
        public string WardNumber { get; set; }

        public bool IsDeleted { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        
        public int? CreatedId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
