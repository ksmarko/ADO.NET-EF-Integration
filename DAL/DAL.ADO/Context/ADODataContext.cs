using DAL.DAL.ADO.Repositories;
using DAL.DAL.EF.Interfaces;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.ADO.Context
{
    public class ADODataContext
    {
        public static IUnitOfWork Create(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            return new ADOUnitOfWork(connection);
        }
    }
}
