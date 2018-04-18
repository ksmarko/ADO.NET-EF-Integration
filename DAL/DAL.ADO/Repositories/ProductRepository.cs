using DAL.DAL.ADO.Context;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Product Get(int id)
        {
            DataRow[] query = _table.Select($"Id = {id}");

            if (query.Length == 0) return null;

            DataRow row = query[0];

            var product = new  Product
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

            query = _context.ProviderProducts.Select($"Product_Id = {product.Id}");

            foreach (DataRow _row in query)
            {
                int providerId = (int)_row["Provider_Id"];

                DataRow[] query1 = _context.Providers.Select($"Id = {providerId}");
                DataRow row1 = query1[0];

                var provider = new Provider
                {
                    Id = (int)row1["Id"],
                    Name = (string)row1["Name"],
                    Location = (string)row1["Location"]
                };

                product.Providers.Add(provider);
            }

            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();

            for (int curRow = 0; curRow < _table.Rows.Count; curRow++)
            {
                var product = new Product
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
                    },

                    Providers = new List<Provider>()
                };

                var providerRows = _context.GetChildRowsFor(_table.Rows[curRow], "ProductProviderProducts");

                for (int i = 0; i < providerRows.Length; i++)
                {
                    if ((int)providerRows[i]["Product_Id"] == product.Id)
                    {
                        int providerId = (int)providerRows[i]["Provider_Id"];

                        for (int j = 0; j < _context.Providers.Rows.Count; j++)
                            if ((int)_context.Providers.Rows[j]["Id"] == providerId)

                                product.Providers.Add(new Provider
                                {
                                    Id = (int)_context.Providers.Rows[j]["Id"],
                                    Name = (string)_context.Providers.Rows[j]["Name"],
                                    Location = (string)_context.Providers.Rows[j]["Location"]
                                });
                    }
                }

                products.Add(product);
            }
            return products;
        }
    }
}
