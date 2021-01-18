using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.UserModule.Models
{
    public class UserManagmentsModel
    {
        public User User { get; set; }
        public bool isAddOperation { get; set; }
        public string LicenseStartDate { get; set; }

        public string LicenseEndDate { get; set; }

        public bool IsActive { get; set; }

        public bool CustomURL { get; set; }

        public bool DataUploadForHazardAndTrade { get; set; }
    }
}
