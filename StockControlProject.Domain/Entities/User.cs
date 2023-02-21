using StockControlProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Orders = new List<Order>();
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhotoURL { get; set; }        
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        //1 User More Orders
        public virtual List<Order> Orders { get; set; }
    }
}
