using Core.ViewService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Company.Models.CompanyAdmin.EditorModel 
{
  public  class CompanyManagementModel : BaseEditorModel
    {
        public CompanyAdmin CompanyAdmin { get; set; }
        public bool isAddOperation { get; set; }
        public string LicenseStartDate { get; set; }

        public string LicenseEndDate { get; set; }

        public bool IsActive { get; set; }

        public bool CustomURL { get; set; }

        public bool DataUploadForHazardAndTrade { get; set; }

    }
}
