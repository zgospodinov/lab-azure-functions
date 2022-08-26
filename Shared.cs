using System;
using System.Data.SqlClient;

namespace azure_functions
{
    internal static class Shared
    {
        internal static SqlConnection GetConnection()
        {
            string connString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SQLConnectionString");
            return new SqlConnection(connString);
        }
    }
}