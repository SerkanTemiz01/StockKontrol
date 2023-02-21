using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {
            Products= new List<Product>();
        }

        public string SupplierName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        // 1 supplier More Products
        public virtual List<Product> Products { get; set; }
    }
}
