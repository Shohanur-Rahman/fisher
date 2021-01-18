using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.CostReport
{
    public class CostReportDomains
    {
        public CostExpandTable costExpandTable { get; set; }
        public bool isAddOperation { get; set; }
    }

    [Table("Expend")]
    public class CostExpandTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(250)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [Display(Name = "Details")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }


    public class CostExpandNamePorperty
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CreaetdDate { get; set; }
        public int TotalRows { get; set; }
    }
}
