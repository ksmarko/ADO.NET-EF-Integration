using DAL.DAL.ADO.Context;
using DAL.DAL.EF.Interfaces;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.ADO.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        ADODataContext _context;
        DataTable _table;

        public ProductRepository(ADODataContext context)
        {
            _context = context;
            _table = _context.Products;
        }

        public void Create(Product item)
        {
            DataRow newRow = _table.NewRow();

            newRow["Name"] = item.Name;
            newRow["Price"] = item.Price;
            newRow["CategoryId"] = item.CategoryId;

            _table.Rows.Add(newRow);
        }

        public void Delete(int id)
        {
            var product = Get(id);

            if (product != null)
            {
                DataRow rowToDelete = _table.Select($"Id = {id}")[0];
                rowToDelete.Delete();
                return;
            }
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            DataRow[] query = _table.Select($"Id = {id}");

            if (query.Length == 0) return null;

            DataRow row = query[0];

            return new Product
            {
                Id = (int)row["Id"],
                Name = (string)row["Name"],
                CategoryId = (int)row["CategoryId"],
                Price = (double)row["Price"],

                Category = new Category
                {
                    Id = (int)_context.GetParentRowFor
                                (row, "CategoryProduct")["Id"],
                    Name = (string)_context.GetParentRowFor
                                (row, "CategoryProduct")["Name"]
                }
            };
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();

            for (int curRow = 0; curRow < _table.Rows.Count; curRow++)
            {
                products.Add(new Product
                {
                    Id = (int)_table.Rows[curRow]["Id"],
                    Name = (string)_table.Rows[curRow]["Name"],
                    CategoryId = (int)_table.Rows[curRow]["CategoryId"],
                    Price = (double)_table.Rows[curRow]["Price"],

                    Category = new Category
                    {
                        Id = (int)_context.GetParentRowFor
                                (_table.Rows[curRow], "CategoryProduct")["Id"],

                        Name = (string)_context.GetParentRowFor
                                (_table.Rows[curRow], "CategoryProduct")["Name"]
                    }
                });
            }

            return products;
        }
    }
}
