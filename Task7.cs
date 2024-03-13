using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace VegetablesAndFruitsApp
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            IDatabaseConnectionFactory connectionFactory = new SqlConnectionFactory("Data Source=(local);Initial Catalog=VegetablesAndFruits;Integrated Security=True");


            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Подключение к базе данных успешно!");

                    string query = "SELECT * FROM Items";
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        Stopwatch t = new Stopwatch();  /// t - время выполнения
                        t.Start(); //// начало отсчета времени

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Название: {reader["Name"]}, Тип: {reader["Type"]}, Цвет: {reader["Color"]}, Калорийность: {reader["Calories"]}");
                            }
                        }

                        t.Stop(); /// остановка отсчета времени
                        Console.WriteLine($"Время выполнения запроса SELECT: {t.Elapsed.TotalSeconds} секунд");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Похоже, овощи и фрукты сегодня на свежие!");
                }
            }
        }
    }
}
