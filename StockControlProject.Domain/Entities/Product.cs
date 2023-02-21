using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }

        // Navigation props
        // 1 Product 1 Category
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }

        // 1 Product 1 Supplier
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; }

        // 1 Product More Order

        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
