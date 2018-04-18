using DAL.DAL.ADO.Context;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.DAL.ADO.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        ADODataContext _context;
        DataTable _table;

        public ProviderRepository(ADODataContext context)
        {
            _context = context;
            _table = _context.Providers;
        }

        public void Create(Provider item)
        {
            DataRow newRow = _table.NewRow();
            newRow["Name"] = item.Name;
            newRow["Location"] = item.Location;
            _table.Rows.Add(newRow);
        }

        public IEnumerable<Provider> Find(Func<Provider, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Provider Get(int id)
        {
            DataRow[] query = _table.Select($"Id = {id}");

            if (query.Length == 0) return null;

            DataRow row = query[0];
            var provider = new Provider
            {
                Id = (int)row["Id"],
                Name = (string)row["Name"],
                Location = (string)row["Location"],
                Products = new List<Product>()
            };

            query = _context.ProviderProducts.Select($"Provider_Id = {provider.Id}");

            foreach(DataRow _row in query)
            {
                int productId = (int)_row["Product_Id"];

                DataRow[] query1 = _context.Products.Select($"Id = {productId}");
                DataRow row1 = query1[0];

                var product = new Product
                {
                    Id = (int)row1["Id"],
                    Name = (string)row1["Name"],
                    Price = (double)row1["Price"],
                    CategoryId = (int)row1["CategoryId"]
                };

                provider.Products.Add(product);
            }

            return provider;
        }

        public IEnumerable<Provider> GetAll()
        {
            var providers = new List<Provider>();

            for (int curRow = 0; curRow < _table.Rows.Count; curRow++)
            {
                var provider = new Provider
                {
                    Id = (int)_table.Rows[curRow]["Id"],
                    Name = (string)_table.Rows[curRow]["Name"],
                    Location = (string)_table.Rows[curRow]["Location"],
                    Products = new List<Product>()
                };

                var productRows = _context.GetChildRowsFor(_table.Rows[curRow], "ProviderProviderProducts");

                for (int i = 0; i < productRows.Length; i++)
                {
                    if ((int)productRows[i]["Provider_Id"] == provider.Id)
                    {
                        int productId = (int)productRows[i]["Product_Id"];

                        for (int j = 0; j < _context.Products.Rows.Count; j++ )
                            if ((int)_context.Products.Rows[j]["Id"] == productId)

                        provider.Products.Add(new Product
                        {
                            Id = (int)_context.Products.Rows[j]["Id"],
                            Name = (string)_context.Products.Rows[j]["Name"],
                            CategoryId = (int)_context.Products.Rows[j]["CategoryId"],
                            Price = (double)_context.Products.Rows[j]["Price"]
                        });
                    }
                }

                providers.Add(provider);
            }

            return providers;
        }
    }
}
