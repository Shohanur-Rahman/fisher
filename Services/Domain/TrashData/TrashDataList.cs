using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.TrashData
{
    public class TrashDataList
    {
        public int RowId { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string TableName { get; set; }
        public int TableID { get; set; }
        public string CreatedDate { get; set; }

    }
}
