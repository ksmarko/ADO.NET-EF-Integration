using System.Collections.Generic;

namespace DAL.Shared.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }        
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Provider> Providers { get; set; }

        public Product()
        {
            Providers = new List<Provider>();
        }
    }
}
