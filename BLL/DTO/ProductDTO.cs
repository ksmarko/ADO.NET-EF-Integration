using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual CategoryDTO Category { get; set; }
        public virtual ICollection<ProviderDTO> Providers { get; set; }

        public override string ToString()
        {
            return string.Join(" ", Name, Price, Category.Name);
        }
    }
}
