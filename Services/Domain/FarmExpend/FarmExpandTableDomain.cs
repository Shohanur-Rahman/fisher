
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Domain.FarmExpend
{
    public class FarmExpandTableDomain
    {
        public ExpendReportTable ExpendReportTable { get; set; }
        public IList<ExpendReportMapper> ExpendReportMapperList { get; set; }
        public string AreaIdListWithComma { get; set; }
        public string ExistingMapperIdListForEdit { get; set; }
        public ExpendReportMapper ExpendReportMapper { get; set; }
        public bool isAddOperation { get; set; }
    }


    [Table("ExpendReport")]
    public class ExpendReportTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpendReportId { get; set; }
        public string ExpendRepoterName { get; set; }
        public int ExpendId { get; set; }
        public decimal TotalExpend { get; set; }
        public DateTime CreateDate { get; set; }   
        public int CreatedId { get; set; }
        public string ExpandNote { get; set; }
    }


    [Table("ExpendReportMapper")]
    public class ExpendReportMapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpendMapperId { get; set; }
        public int ExpendReportId { get; set; }
        public int AreaId { get; set; }
        public decimal ExpendAmount { get; set; }
    }

    

}
