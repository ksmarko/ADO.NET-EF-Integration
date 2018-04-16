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
        public DataTable ProviderProducts { get; }

        private DataSet _db = new DataSet("Catalog");

        private SqlCommandBuilder _sqlCbProducts;
        private SqlCommandBuilder _sqlCbCategories;
        private SqlCommandBuilder _sqlCbProviders;
        private SqlCommandBuilder _sqlCbProviderProducts;

        private SqlDataAdapter _productsTableAdapter;
        private SqlDataAdapter _categoriesTableAdapter;
        private SqlDataAdapter _providersTableAdapter;
        private SqlDataAdapter _providerProductsTableAdapter;

        private string _connectionString;

        public ADODataContext(string connectionString)
        {
            _connectionString = connectionString;

            InitializeDb();

            _productsTableAdapter = new SqlDataAdapter("SELECT * FROM Products", _connectionString);
            _categoriesTableAdapter = new SqlDataAdapter("SELECT * FROM Categories", _connectionString);
            _providersTableAdapter = new SqlDataAdapter("SELECT * FROM Providers", _connectionString);
            _providerProductsTableAdapter = new SqlDataAdapter("SELECT * FROM ProviderProducts", _connectionString);

            _sqlCbProducts = new SqlCommandBuilder(_productsTableAdapter);
            _sqlCbCategories = new SqlCommandBuilder(_categoriesTableAdapter);
            _sqlCbProviders = new SqlCommandBuilder(_providersTableAdapter);
            _sqlCbProviderProducts = new SqlCommandBuilder(_providerProductsTableAdapter);

            _productsTableAdapter.Fill(_db, "Products");
            _categoriesTableAdapter.Fill(_db, "Categories");
            _providersTableAdapter.Fill(_db, "Providers");
            _providerProductsTableAdapter.Fill(_db, "ProviderProducts");

            BuildTableRelationship();

            Products = _db.Tables["Products"];
            Categories = _db.Tables["Categories"];
            Providers = _db.Tables["Providers"];
            ProviderProducts = _db.Tables["ProviderProducts"];
        }

        public void SaveChanges()
        {
            try
            {
                _productsTableAdapter.Update(_db, "Products");
                _categoriesTableAdapter.Update(_db, "Categories");
                _providersTableAdapter.Update(_db, "Providers");
                _providerProductsTableAdapter.Update(_db, "ProviderProducts");
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

                new DataRelation("ProviderProviderProducts",
                _db.Tables["Providers"].Columns["Id"],
                _db.Tables["ProviderProducts"].Columns["Provider_Id"]),

                new DataRelation("ProductProviderProducts",
                _db.Tables["Products"].Columns["Id"],
                _db.Tables["ProviderProducts"].Columns["Product_Id"]),
            });
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private void InitializeDb()
        {
            //drop create db +
            //create tales 
            //build the table relations
            //fill the tables
            //save changes
            string connString = @"data source = (localdb)\MSSQLLocalDB; Integrated Security = True; MultipleActiveResultSets = True";
            string dbName = new SqlConnection(_connectionString).Database;

            using (var connection = new SqlConnection(connString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{dbName}')", connection))
                {
                    connection.Open();
                    if (command.ExecuteScalar() != DBNull.Value) //database exists
                    {
                        string query = "Use master DROP DATABASE [" + dbName + "]";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = "Use master Create DATABASE [" + dbName + "]";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            //string sqlExpression = "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Laptop X', 666000, 1)";

            //using (SqlConnection connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //    int number = command.ExecuteNonQuery();
            //}

            SaveChanges();
        }
    }
}
