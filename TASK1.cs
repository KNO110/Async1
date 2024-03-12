using System;
using System.Data;
using System.Data.SqlClient;

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
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Название: {reader["Name"]}, Тип: {reader["Type"]}, Цвет: {reader["Color"]}, Калорийность: {reader["Calories"]}");
                            }
                        }
                    }

                    string insertQuery = "INSERT INTO Items (Name, Type, Color, Calories) VALUES (@Name, @Type, @Color, @Calories)";
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = insertQuery;
                        command.Parameters.Add(new SqlParameter("@Name", "Новый продукт"));
                        command.Parameters.Add(new SqlParameter("@Type", "Фрукт"));
                        command.Parameters.Add(new SqlParameter("@Color", "Желтый"));
                        command.Parameters.Add(new SqlParameter("@Calories", 100));
                        command.ExecuteNonQuery();
                        Console.WriteLine("Новая запись успешно добавлена!");
                    }

                    string updateQuery = "UPDATE Items SET Calories = @Calories WHERE Name = @Name";
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = updateQuery;
                        command.Parameters.Add(new SqlParameter("@Calories", 120));
                        command.Parameters.Add(new SqlParameter("@Name", "Новый продукт"));
                        command.ExecuteNonQuery();
                        Console.WriteLine("Запись успешно обновлена!");
                    }

                    string deleteQuery = "DELETE FROM Items WHERE Name = @Name";
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = deleteQuery;
                        command.Parameters.Add(new SqlParameter("@Name", "Новый продукт"));
                        command.ExecuteNonQuery();
                        Console.WriteLine("Запись успешно удалена!");
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
