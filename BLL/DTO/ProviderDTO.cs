using System.Collections.Generic;

namespace BLL.DTO
{
    public class ProviderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<ProductDTO> Products { get; set; }

        public override string ToString()
        {
            return string.Join(" ", Name, "(" + Location + ")");
        }
    }
}
