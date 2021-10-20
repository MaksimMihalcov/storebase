using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST_ASP_APP.Models
{
    public class Product
    {
        [Key] public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
