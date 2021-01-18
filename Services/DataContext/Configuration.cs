using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Services.DataContext
{

    //This code added by  for retry of sql execution after failure
    public class MyConfiguration  : DbConfiguration
    {
        public MyConfiguration ()
        {
            SetExecutionStrategy(
           "System.Data.SqlClient",
           () => new SqlAzureExecutionStrategy(100, TimeSpan.FromSeconds(30)));
        }
    }
}
