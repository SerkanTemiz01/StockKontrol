using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class OrderDetails : BaseEntity
    {
        
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }

        // 1 Orderdetails 1 Product
        // 1 Orderdetails 1 Order
        
        public virtual Product? Product { get; set; } = null!;
        public virtual Order? Order { get; set; } = null!;
    }
}
