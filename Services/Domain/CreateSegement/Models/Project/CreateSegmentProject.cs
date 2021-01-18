using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CreateSegement.Models.Project
{
    [Table("Project")]
    public class CreateSegmentProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Area Name")]
        public string Name { get; set; }
        
        public int AreaId { get; set; }
        
        public string Land { get; set; }

        public string Cost { get; set; }

        public string Time { get; set; }
        
        public string ImageUrl { get; set; }

        public int? CreatedId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}
