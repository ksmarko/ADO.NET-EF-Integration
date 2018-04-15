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

        public void Delete(int id)
        {
            var supplier = Get(id);

            if (supplier != null)
            {
                DataRow rowToDelete = _table.Select($"Id = {id}")[0];
                rowToDelete.Delete();
                return;
            }
        }

        public IEnumerable<Provider> Find(Func<Provider, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Provider Get(int id)
        {
            DataRow[] query = _table.Select($"Id = {id}");

            if (query.Length == 0) return null;

            DataRow row = query[0];
            var supplier = new Provider
            {
                Id = (int)row["Id"],
                Name = (string)row["Name"],
                Location = (string)row["Location"],
                Products = new List<Product>()
            };

            //var productRows = _context.GetChildRowsFor
            //                      (row, "ProviderProduct");

            //for (int i = 0; i < productRows.Length; i++)
            //{
            //    supplier.Products.Add(new Product
            //    {
            //        Id = (int)productRows[i]["Id"],
            //        Name = (string)productRows[i]["Name"],
            //        CategoryId = (int)productRows[i]["CategoryId"],
            //        Price = (double)productRows[i]["Price"]
            //    });
            //}

            return supplier;
        }

        public IEnumerable<Provider> GetAll()
        {
            var providers = new List<Provider>();

            for (int curRow = 0; curRow < _table.Rows.Count; curRow++)
            {
                var supplier = new Provider
                {
                    Id = (int)_table.Rows[curRow]["Id"],
                    Name = (string)_table.Rows[curRow]["Name"],
                    Location = (string)_table.Rows[curRow]["Location"],
                    Products = new List<Product>()
                };

                //var productRows = _context.GetChildRowsFor
                //                  (_table.Rows[curRow], "ProviderProduct");

                //for (int i = 0; i < productRows.Length; i++)
                //{
                //    supplier.Products.Add(new Product
                //    {
                //        Id = (int)productRows[i]["Id"],
                //        Name = (string)productRows[i]["Name"],
                //        CategoryId = (int)productRows[i]["CategoryId"],
                //        Price = (double)productRows[i]["Price"]
                //    });
                //}

                providers.Add(supplier);
            }

            return providers;
        }
    }
}
