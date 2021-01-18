using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Company.SPModels
{
    public class SMDLCompanyAdminList
    {
       
      
        public long companyID { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string IsActive { get; set; }
        public int TotalRows { get; set; }

    }
}
