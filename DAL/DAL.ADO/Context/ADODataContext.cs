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
            InitializeTables();

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
            string connString = @"data source = (localdb)\MSSQLLocalDB; Integrated Security = True; MultipleActiveResultSets = True";
            string dbName = new SqlConnection(_connectionString).Database;

            //drop create db
            using (var connection = new SqlConnection(connString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{dbName}')", connection))
                {
                    connection.Open();
                    if (command.ExecuteScalar() != DBNull.Value) //database exists
                    {
                        //todo: close all connections
                        string q = "Use master DROP DATABASE [" + dbName + "]";
                        SqlCommand c = new SqlCommand(q, connection);
                        c.ExecuteNonQuery();
                    }

                    string query = "Create DATABASE [" + dbName + "]";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }

            //creating tables
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string[] queries =
                {
                    "Create table [dbo].[Categories] ([Id] [int] IDENTITY(1,1) NOT NULL, [Name][nvarchar](max) NULL, CONSTRAINT[PK_dbo.Categories] PRIMARY KEY CLUSTERED ([Id] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]",
                    "CREATE TABLE [dbo].[Products]([Id][int] IDENTITY(1, 1) NOT NULL, [Name] [nvarchar](max) NULL, [Price] [float] NOT NULL,[CategoryId] [int] NULL, CONSTRAINT[PK_dbo.Products] PRIMARY KEY CLUSTERED([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]",
                    "ALTER TABLE[dbo].[Products] WITH CHECK ADD CONSTRAINT[FK_dbo.Products_dbo.Categories_CategoryId] FOREIGN KEY([CategoryId]) REFERENCES[dbo].[Categories]([Id])",
                    "ALTER TABLE[dbo].[Products] CHECK CONSTRAINT[FK_dbo.Products_dbo.Categories_CategoryId]",
                    "CREATE TABLE [dbo].[Providers]([Id][int] IDENTITY(1, 1) NOT NULL, [Name] [nvarchar](max) NULL,[Location] [nvarchar](max) NULL, CONSTRAINT[PK_dbo.Providers] PRIMARY KEY CLUSTERED([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]",
                    "CREATE TABLE [dbo].[ProviderProducts]([Provider_Id][int] NOT NULL, [Product_Id] [int] NOT NULL,CONSTRAINT[PK_dbo.ProviderProducts] PRIMARY KEY CLUSTERED ( [Provider_Id] ASC, [Product_Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]",
                    "ALTER TABLE[dbo].[ProviderProducts] WITH CHECK ADD CONSTRAINT[FK_dbo.ProviderProducts_dbo.Products_Product_Id] FOREIGN KEY([Product_Id])REFERENCES[dbo].[Products] ([Id]) ON DELETE CASCADE",
                    "ALTER TABLE[dbo].[ProviderProducts] CHECK CONSTRAINT[FK_dbo.ProviderProducts_dbo.Products_Product_Id]",
                    "ALTER TABLE[dbo].[ProviderProducts] WITH CHECK ADD CONSTRAINT[FK_dbo.ProviderProducts_dbo.Providers_Provider_Id] FOREIGN KEY([Provider_Id]) REFERENCES[dbo].[Providers] ([Id]) ON DELETE CASCADE",
                    "ALTER TABLE[dbo].[ProviderProducts] CHECK CONSTRAINT[FK_dbo.ProviderProducts_dbo.Providers_Provider_Id]"
                };

                foreach (var query in queries)
                {
                    var cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InitializeTables()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string[] sqlExpressions =
                {
                "INSERT INTO Categories (Name) VALUES ('Electronics')",
                "INSERT INTO Categories (Name) VALUES ('Food')",
                "INSERT INTO Categories (Name) VALUES ('Clothes')",
                "INSERT INTO Categories (Name) VALUES ('Sport')",

                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Laptop', 35000, 1)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('iPhone6s', 16000, 1)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Chips', 30, 2)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Beer', 20, 2)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Bicycle', 40000, 4)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('Dress', 600, 3)",
                "INSERT INTO Products (Name, Price, CategoryId) VALUES ('T-shirt', 120, 3)",
                
                "INSERT INTO Providers (Name, Location) VALUES ('Apple', 'USA')",
                "INSERT INTO Providers (Name, Location) VALUES ('BMW', 'Germany')",
                "INSERT INTO Providers (Name, Location) VALUES ('Lays', 'Ukraine')",
                "INSERT INTO Providers (Name, Location) VALUES ('OJJI', 'Russia')",
                "INSERT INTO Providers (Name, Location) VALUES ('Baltyka', 'Ukraine')",

                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (1, 1)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (1, 2)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (3, 3)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (5, 4)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (2, 5)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (4, 6)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (2, 7)",
                "INSERT INTO ProviderProducts (Provider_Id, Product_Id) VALUES (4, 7)"
                };

                foreach (var el in sqlExpressions)
                {
                    SqlCommand command = new SqlCommand(el, connection);
                    int number = command.ExecuteNonQuery();
                }
            }

            //SaveChanges();
        }
    }
}
