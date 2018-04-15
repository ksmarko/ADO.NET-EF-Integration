using DAL.DAL.ADO.Repositories;
using DAL.DAL.EF.Interfaces;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.ADO.Context
{
    public class ADODataContext
    {
        public DataTable Products { get; }
        public DataTable Categories { get; }
        public DataTable Providers { get; }

        private DataSet _db = new DataSet("Catalog");

        private SqlCommandBuilder _sqlCbProducts;
        private SqlCommandBuilder _sqlCbCategories;
        private SqlCommandBuilder _sqlCbProviders;

        private SqlDataAdapter _productsTableAdapter;
        private SqlDataAdapter _categoriesTableAdapter;
        private SqlDataAdapter _providersTableAdapter;
        private string _connectionString;

        public ADODataContext(string connectionString)
        {
            _connectionString = ConfigurationManager
                   .ConnectionStrings["DefaultConnection"]
                   .ConnectionString;

            _productsTableAdapter = new SqlDataAdapter("SELECT * FROM Products", _connectionString);
            _categoriesTableAdapter = new SqlDataAdapter("SELECT * FROM Categories", _connectionString);
            _providersTableAdapter = new SqlDataAdapter("SELECT * FROM Providers", _connectionString);

            _sqlCbProducts = new SqlCommandBuilder(_productsTableAdapter);
            _sqlCbCategories = new SqlCommandBuilder(_categoriesTableAdapter);
            _sqlCbProviders = new SqlCommandBuilder(_providersTableAdapter);

            _productsTableAdapter.Fill(_db, "Products");
            _categoriesTableAdapter.Fill(_db, "Categories");
            _providersTableAdapter.Fill(_db, "Providers");

            BuildTableRelationship();

            Products = _db.Tables["Products"];
            Categories = _db.Tables["Categories"];
            Providers = _db.Tables["Providers"];
        }

        public void SaveChanges()
        {
            try
            {
                _productsTableAdapter.Update(_db, "Products");
                _categoriesTableAdapter.Update(_db, "Categories");
                _providersTableAdapter.Update(_db, "Providers");
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (DBConcurrencyException ex)
            {
                throw ex;
            }
        }

        public DataRow GetParentRowFor(DataRow row, string relationName)
        {
            return row.GetParentRow(_db.Relations[relationName]);
        }

        public DataRow[] GetChildRowsFor(DataRow row, string relationName)
        {
            return row.GetChildRows(_db.Relations[relationName]);
        }

        private void BuildTableRelationship()
        {
            _db.Relations.AddRange(new[]
            {
                new DataRelation("CategoryProduct",
                _db.Tables["Categories"].Columns["Id"],
                _db.Tables["Products"].Columns["CategoryId"]),

                //new DataRelation("ProviderProduct",
                //_db.Tables["Providers"].Columns["Id"],
                //_db.Tables["Products"].Columns["ProviderId"])
            });
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
