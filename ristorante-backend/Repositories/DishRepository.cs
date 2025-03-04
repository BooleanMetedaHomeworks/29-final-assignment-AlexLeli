using System.Data.Common;
using Microsoft.Data.SqlClient;
using ristorante_backend.Models;

namespace ristorante_backend.Repositories
{
    public class PostRepository
    {
        private const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=GestionaleRistorante;Integrated Security=True;Trust Server Certificate=True";


        public void ReadDish(SqlDataReader r, Dictionary<int, Dish> dishes)
        {
            try
            {
                int id = r.GetInt32(r.GetOrdinal("ID_Dish"));

                Dish p = new Dish();
                if (dishes.ContainsKey(id) == false)
                {
                    p.ID_Dish = id;
                    p.Name = r.GetString(r.GetOrdinal("Name"));
                    p.Description = r.GetString(r.GetOrdinal("Description"));
                    p.Price = r.GetDecimal(r.GetOrdinal("Price"));
                    if (r.IsDBNull(r.GetOrdinal("ID_Category")) == false)
                    {
                        int categoryId = r.GetInt32(r.GetOrdinal("ID_Category"));
                        string categoryName = r.GetString(r.GetOrdinal("CategoryName"));
                        Category c = new Category(categoryId, categoryName);

                        p.ID_Category = c.ID_Category;
                        p.Category = c;
                    }
                    dishes.Add(id, p);
                }
                p = dishes[id];
                int menuId = r.GetInt32(r.GetOrdinal("ID_Menu"));
                p.MenuIDs.Add(menuId);
                string menuName = r.GetString(r.GetOrdinal("MenuName"));
                Menu m = new Menu(menuId, menuName);


                p.Menus.Add(m);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
                throw new Exception(e.Message, e.InnerException);
            }
        }








        public async Task<List<Dish>> GetAllDishes()
        {
            List<Dish> allDishes = new List<Dish>();
            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();

            string query = @"SELECT d.*, c.ID_Category, c.Name AS CategoryName, m.ID_Menu, m.Name AS MenuName
							FROM Dishes d
						    LEFT JOIN Categories c ON d.ID_Category = c.ID_Category
							LEFT JOIN Dish_Menu dm ON d.ID_Dish = dm.ID_Dish
						    LEFT JOIN Menu m ON dm.ID_Menu = m.ID_Menu";

            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    ReadDish(reader, dishes);
                                }
                            }

                            foreach (Dish d in dishes.Values)
                            {
                                allDishes.Add(d);
                            }               
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
            }

            return allDishes;
        }








        public async Task<Dish> GetDish(int id)
        {
            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();

            string query = @"SELECT d.*, c.ID_Category, c.Name AS CategoryName, m.ID_Menu, m.Name AS MenuName
							FROM Dishes d
						    LEFT JOIN Categories c ON d.ID_Dish = c.ID_Category
							LEFT JOIN Dish_Menu dm ON d.ID_Dish = dm.ID_Dish
						    LEFT JOIN Menu m ON dm.ID_Menu = m.ID_Menu
							WHERE d.ID_Dish = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                       
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    ReadDish(reader, dishes);
                                }
                            }  

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");


            }

            return dishes.Values.FirstOrDefault();
        }










        public async Task<List<Dish>> GetDishesByName(string name)
        {
            List<Dish> allNamedDishes = new List<Dish>();
            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();
            string query = "SELECT * FROM Dishes WHERE Name LIKE @Name";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", $"%{name}%");
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ReadDish(reader, dishes);
                        }

                        foreach (Dish d in dishes.Values)
                        {
                            allNamedDishes.Add(d);
                        }
                    }
                }
            }
            return allNamedDishes;
        }



  





        public async Task<(int dishId, int affectedRowsBridgeTable)> CreateDish(Dish dish)
        {
            string query = @"INSERT INTO Dishes (Name, Description, Price, ID_Category) VALUES (@Name, @Description, @Price, @ID_Category);
						    SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        using (SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync())
                        {
                            command.Transaction = transaction;

                            try
                            {
                                command.CommandText = query;
                                command.Parameters.AddWithValue("@Name", dish.Name);
                                command.Parameters.AddWithValue("@Description", dish.Description);
                                command.Parameters.AddWithValue("@Price", dish.Price);
                                command.Parameters.AddWithValue("@ID_Category", dish.ID_Category ?? (object)DBNull.Value);
                                int dishId = Convert.ToInt32(await command.ExecuteScalarAsync());

                                
                                int affectedRowsBridgeTable = await AddDishMenu(dishId, dish.MenuIDs, connection, transaction);

                                await transaction.CommitAsync();

                                return (dishId, affectedRowsBridgeTable);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}")
                                await transaction.RollbackAsync();
                                throw new Exception(e.Message, e.InnerException);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}")
            }            

            return default;
                         
        }



                                













        public async Task<int> UpdateDish(int id, Dish dish)
        {
            string query = "UPDATE Dishes SET Name = @Name, Description = @Description, Price = @Price WHERE ID_Dish = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        using (SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync())
                        {
                            command.Transaction = transaction;

                            try
                            {
                                command.CommandText = query;
                                command.Parameters.AddWithValue("@Id", id);
                                command.Parameters.AddWithValue("@Name", dish.Name);
                                command.Parameters.AddWithValue("@Description", dish.Description);
                                command.Parameters.AddWithValue("@Price", dish.Price);
                                int rowsAffected = await command.ExecuteNonQueryAsync();



                                return rowsAffected;
                            }

                            catch (Exception e)
                            {
                                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
                                await transaction.RollbackAsync();
                                throw new Exception(e.Message, e.InnerException);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
            }

            return default;
        }










        public async Task<int> DeleteDish(int id)
        {

            string query = "DELETE FROM Dishes WHERE ID_Dish = @Id";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        using (SqlTransaction transaction = (SqlTransaction) await connection.BeginTransactionAsync())
                        {
                            command.Transaction = transaction;

                            try
                            {
                                command.CommandText = query;
                                command.Parameters.AddWithValue("@Id", id);
                                return await command.ExecuteNonQueryAsync();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}")
                                await transaction.RollbackAsync();
                                throw new Exception(e.Message, e.InnerException);
                            }                        
                        } 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}")
            }

            return default;
            
        }






        private async Task<int> AddDishMenu(int dishId, List<int> menuIds, SqlConnection connection, SqlTransaction transaction)
        {

            int affectedRows = 0;

            foreach (int menuId in menuIds)
            {
                string query = @"INSERT INTO Dish_Menu (ID_Dish, ID_Menu)
                                        VALUES (@ID_Dish, @ID_Menu)";
                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@ID_Dish", dishId);
                    cmd.Parameters.Add(new SqlParameter("@ID_Menu", menuId));
                    affectedRows += await cmd.ExecuteNonQueryAsync();
                }
            }
            return affectedRows;
        }


        private async Task<int> UpdateDishMenu(int dishId, List<int> menuIds, SqlConnection connection)
        {

            int affectedRows = 0;

            foreach (int menuId in menuIds)
            {
                string query = @"UPDATE Dish_Menu SET ID_Menu = @ID_Menu WHERE ID_Dish =
                                        ;
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_Dish", dishId));
                    cmd.Parameters.Add(new SqlParameter("@ID_Menu", menuId));
                affectedRows = await cmd.ExecuteNonQueryAsync();
            }
        }
            return affectedRows;
        }



}       
}


