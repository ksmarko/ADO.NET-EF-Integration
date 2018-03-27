using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Shared.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        //many to many
        public virtual ICollection<Product> Products { get; set; }

        public Provider()
        {
            Products = new List<Product>();
        }
    }
}
