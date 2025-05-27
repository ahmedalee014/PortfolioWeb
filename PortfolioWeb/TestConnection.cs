using System;
using Microsoft.Data.SqlClient;

namespace PortfolioWeb
{
    class Program
    {
        static void Main()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=PortfolioWeb;Trusted_Connection=True;";

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Success! Connected to:");
                Console.WriteLine($"Server: {connection.DataSource}");
                Console.WriteLine($"Database: {connection.Database}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}