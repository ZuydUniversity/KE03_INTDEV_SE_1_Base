using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FeaturedProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public string Unit { get; set; }
        //public int? ProductId { get; set; }
        //public Product? Product { get; set; }
    }
}

