using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace VegetablesAndFruitsApp    ///в который раз
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = GetConnectionString("VegetablesAndFruits", "(local)");

            Console.WriteLine("Выберите СУБД ( 1 - sqlserver или 2 - sqlite(ибо делать их больше мне лень)):");
            string db_type = Console.ReadLine();

            switch (db_type.ToLower())
            {
                case "sqlserver" or "1":
                    connectionString = GetConnectionString("VegetablesAndFruits", "(local)");
                    break;
                case "sqlite" or "2":
                    connectionString = GetConnectionStringForSQLite();
                    break;
                default:
                    Console.WriteLine("Ты что-то напутал (точно не я, я ведь талант), короче будет бд по стандарту (это sql server).");
                    connectionString = GetConnectionString("VegetablesAndFruits", "(local)");
                    break;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Подключение к базе данных успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static string GetConnectionString(string databaseName, string dataSource)
        {
            return $"Data Source={dataSource};Initial Catalog={databaseName};Integrated Security=True";
        }

        static string GetConnectionStringForSQLite()
        {
            return "Data Source=mydatabase.db;Version=3;";
        }
    }
}
