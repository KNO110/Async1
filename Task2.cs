using System.Configuration;
using System.Data.SqlClient;

namespace VegetablesAndFruitsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["VegetablesAndFruitsConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Работает! Бд подключено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Похоже, разраб даун.");
                }
            }
        }
    }
}
