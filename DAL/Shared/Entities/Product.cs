using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Shared.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        //one to many
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //many to many
        public virtual ICollection<Provider> Providers { get; set; }

        public Product()
        {
            Providers = new List<Provider>();
        }
    }
}
