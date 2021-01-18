using Services.Domain.CreateSegement.Models.Feed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Accounts.Models.FeedDistribute
{


    public class FeedDistributionTableDataModal
    {
        public FeedDistributionTableDoamain feedDistributionTableDoamain { get; set; }
        public IList<FeedDistributionFeedIdReportMapperTable> feedIdAndMapperIdTable { get; set; }
        public IList<FeedDistributionReportMapperTable> feedMapperTable { get; set; }
        public string FeedDistribuinIdsList { get; set; }
        public string DistributionMapperIdList { get; set; }
        public bool isAddOperation { get; set; }
    }

    [Table("FeedDistributionFeedIdReportMapper")]
    public class FeedDistributionFeedIdReportMapperTable {
        public int FeedDistributionFeedIdReportMapperId { get; set; }
        public int FeedDistributionReportId { get; set; }
        public int FeedDistributionFeedId { get; set; }
    }


    [Table("FeedDistributionReportMapper")]
    public class FeedDistributionReportMapperTable
    {
        public int FeedDistributionReportMapperId { get; set; }
        public int FeedDistributionFeedIdReportMapperId { get; set; }
        public int FeedDistributionFeedategoryId { get; set; }
        public int FeedDistributionQuantityId { get; set; }
        public int FeedDistributionSakNumber { get; set; }
        public int FeedDistributionTotalWeight { get; set; }
    }

        [Table("FeedDistributionReports")]
    public class FeedDistributionTableDoamain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedDistributionReportId { get; set; }
        public string FeedDistributionName { get; set; }
        public DateTime FeedDistributionDate { get; set; }
        public Decimal FeedDistributionTotalWeight { get; set; }
        public string FeedDistributionDescription { get; set; }
        public int AreaId { get; set; }
        public int ProjectId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? EditedId { get; set; }
        public DateTime? EditedDate { get; set; }
    }


    public class AllFeedCategories {
        public IList<CreateSegmentFeedCategories> SingleList { get; set; }
    }

    public class SPFeedDistributionReportsBySearchParam
    {
        public int FeedDistributionReportId { get; set; }
        public string FeedDistributionName { get; set; }
        public decimal FeedDistributionQuantityId { get; set; }
        public string FeedDistributionDate { get; set; }
        public string FeedCategoryName { get; set; }
        public string FeedDistributionSakNumber { get; set; }
        public string FeedDistributionTotalWeight { get; set; }
        public string CreatedDate { get; set; }
        public string FeedNameWithCat { get; set; }
        public string ProjectName { get; set; }
        public int TotalRows { get; set; }
    }


    public class up_GetFeedDistributionPieChart {
        public string TotalWeight { get; set; }
        public string AreaName { get; set; }
    }



}
