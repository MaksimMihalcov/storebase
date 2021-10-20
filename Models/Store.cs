using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST_ASP_APP.Models
{
    public class Store
    {
        [Key] public int StoreId { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public TimeSpan Opening_Time { get; set; }
        public TimeSpan Closing_Time { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
