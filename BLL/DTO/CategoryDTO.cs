using System.Collections.Generic;

namespace BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductDTO> Products { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
