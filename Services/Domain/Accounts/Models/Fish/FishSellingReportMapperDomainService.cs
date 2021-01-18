using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Accounts.Models.Fish
{
    [Table("FishSellingReportMapper")]
    public class FishSellingReportMapperTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FishSellMapperId { get; set; }
        public int FishSellReportId { get; set; }
        public int FishSellerId { get; set; }
        public int SellFishId { get; set; }
        public decimal TotalFishkg { get; set; }
        public int PiecesPerKG { get; set; }
        public int TotalPiecesFish { get; set; }
        public decimal PricePerKG { get; set; }
        public decimal TotalSellPrice { get; set; }

    }


    [Table("FishAmountDueReportMapper")]
    public class FishAmountDueReportMapperTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FishSellerReportMapperId { get; set; }
        public int FishSellerId { get; set; }
        public int FishSellReportId { get; set; }
        public decimal FishAmountDue { get; set; }
        
    }

}
