using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Accounts.Models.FarmRents
{
    public class FarmRentsCostsTableDomain
    {
        public FarmRentsCostsTable farmRentsCostsTable { get; set; }
        public bool isAddOperation { get; set; }
    }


    [Table("FarmRentalReports")]
    public class FarmRentsCostsTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FarmRentalReportID { get; set; }
        public string FarmRentalReportName { get; set; } 
        public DateTime FarmRentalDate { get; set; }
        //public int FarmRentalProjectID { get; set; }
        public string FarmRentalDetails { get; set; }
        public decimal FarmRentalLandAmount { get; set; }
        public decimal FarmRentalServieFee { get; set; }
        public decimal FarmRentalMainFee { get; set; }
        public decimal FarmRentalTotalFee { get; set; }
        public decimal FarmRentalCostPerAmount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedID { get; set; } 
        public int ProjectId { get; set; }
        public int AreaId { get; set; }
    }


    public class SPFarmRentalReportsBySearchParam {

        public int FarmRentalReportID { get; set; }
        public string FarmRentalReportName { get; set; }
        public string FarmRentalDetails { get; set; }
        public string FarmRentalDate { get; set; }
        public string ProjectName { get; set; }
        public string FarmRentalLandAmount { get; set; }
        public string FarmRentalMainFee { get; set; }
        public string FarmRentalTotalFee { get; set; }
        public string FarmRentalServieFee { get; set; }
        public string CreatedDate { get; set; }

    }

}
