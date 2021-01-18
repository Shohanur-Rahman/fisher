using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.SPModels
{
    public class SPMDLAdminUserList
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string IsActivated { get; set; }
        public long TotalRows { get; set; }
    }
}
