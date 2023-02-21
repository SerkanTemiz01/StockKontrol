using StockControlProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }

        //Nav Prop
        // 1 Order 1 User
        // 1 Order More OrderDetails
        [ForeignKey("User")]
        public int UserID { get; set; }
        public Status Status { get; set; }
        public virtual User? User { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
