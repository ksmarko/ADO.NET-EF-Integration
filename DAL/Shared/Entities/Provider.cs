using System.Collections.Generic;

namespace DAL.Shared.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Provider()
        {
            Products = new List<Product>();
        }
    }
}
