using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data
{
    public class StockReportItem
    {
        public int StockID { get; set; }
        public string ProductName { get; set; }
        public string WarehouseLocation { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }
}
