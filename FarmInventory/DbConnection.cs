using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FarmInventory
{
    internal static class DbConnection
    {
        // Use Lazy<T> to initialize database connection when it is first accessed
        private static readonly Lazy<NpgsqlConnection> lazyConnection = new Lazy<NpgsqlConnection>(() =>
        {
            NpgsqlConnection connection = new NpgsqlConnection(getConnectionString());
            connection.Open();
            return connection; // return single instance of connection
        });

        // Method to construct and return the connection string for PostgreSQL database connection
        private static string getConnectionString()
        {
            // Define the values of server host, port number, database name, username, and password.  
            // Values maybe different for different individual's system
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=vanierAECWinter2023;";
            string username = "Username=postgres;";
            string password = "Password=CharlineUutm@59394;"; // key in your PostgreSQL password

            // Combine the strings above database connection string
            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, username, password);
            return connectionString;
        }

        public static NpgsqlConnection Connection => lazyConnection.Value; // access to the lazily initialized database connection

        // Method to be used in other classes for single database connection
        public static NpgsqlCommand CreateCommand()
        {
            return Connection.CreateCommand(); 
        }
    }
}
