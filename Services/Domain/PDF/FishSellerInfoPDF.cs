using Services.Domain.Accounts.Models.Fish;
using Services.Domain.CreateSegement.Models.Fish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.PDF
{
    public class FishSellerInfoPDF
    {
        public SellerInfo Seller { get; set; }
        public FishInfo Fish{ get; set; }
        public List<SellInfo> Sell { get; set; }
        public SellInfoTotal TotalSell { get; set; }
        public FishSellingReportTableDomain fishReport { get; set; }
    }


    public class SellInfoTotal {
        public string TotalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
    }

    public class SellerInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class FishInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }

    public class SellInfo
    {
        public string FishName { get; set; }
        public string TotalFishkg { get; set; }
        public string TotalSellPrice { get; set; }
    }



    public class FishSellingReportPDFInfo
    {
        public List<SPFishSellingReportListForAdminBySearchParam> Sell { get; set; }
        public SPFishSellingReportGetTotalForAdminRecord TotalSell { get; set; }
    }
   
}
