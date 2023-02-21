using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new List<Product>();
        }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;

        // NAV prop
        // 1 Category More Product
        public virtual List<Product> Products { get; set; }
    }
}
