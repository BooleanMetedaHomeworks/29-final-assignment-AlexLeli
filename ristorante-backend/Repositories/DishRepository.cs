using System.Data.Common;
using Microsoft.Data.SqlClient;
using ristorante_backend.Models;

namespace ristorante_backend.Repositories
{
    public class DishRepository
    {
        private const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=GestionaleRistorante;Integrated Security=True;Trust Server Certificate=True";


        public void ReadDish(SqlDataReader r, Dictionary<int, Dish> dishes)
        {
            try
            {
                int id = r.GetInt32(r.GetOrdinal("ID_Dish"));

                Dish d = new Dish();
                if (dishes.ContainsKey(id) == false)
                {
                    d.ID_Dish = id;
                    d.Name = r.GetString(r.GetOrdinal("Name"));
                    d.Description = r.GetString(r.GetOrdinal("Description"));
                    d.Price = r.GetDecimal(r.GetOrdinal("Price"));
                    if (r.IsDBNull(r.GetOrdinal("ID_Category")) == false)
                    {
                        int categoryId = r.GetInt32(r.GetOrdinal("ID_Category"));
                        string categoryName = r.GetString(r.GetOrdinal("CategoryName"));
                        Category c = new Category(categoryId, categoryName);

                        d.ID_Category = c.ID_Category;
                        d.Category = c;
                    }
                    dishes.Add(id, d);
                }
                d = dishes[id];
                int menuId = r.GetInt32(r.GetOrdinal("ID_Menu"));
                d.MenuIDs.Add(menuId);
                string menuName = r.GetString(r.GetOrdinal("MenuName"));
                Menu m = new Menu(menuId, menuName);


                d.Menus.Add(m);
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
						    LEFT JOIN Menus m ON dm.ID_Menu = m.ID_Menu";

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
                throw e;
            }

            return allDishes;
        }








        public async Task<Dish> GetDish(int id)
        {
            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();

            string query = @"SELECT d.*, c.ID_Category, c.Name AS CategoryName, m.ID_Menu, m.Name AS MenuName
							FROM Dishes d
						    LEFT JOIN Categories c ON d.ID_Category = c.ID_Category
							LEFT JOIN Dish_Menu dm ON d.ID_Dish = dm.ID_Dish
						    LEFT JOIN Menus m ON dm.ID_Menu = m.ID_Menu
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
                throw e;

            }

            return dishes.Values.FirstOrDefault();
        }










        public async Task<List<Dish>> GetDishesByName(string name)
        {
            List<Dish> allNamedDishes = new List<Dish>();
            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();
            string query = @"SELECT d.*, c.ID_Category, c.Name AS CategoryName, m.ID_Menu, m.Name AS MenuName
							FROM Dishes d
						    LEFT JOIN Categories c ON d.ID_Category = c.ID_Category
							LEFT JOIN Dish_Menu dm ON d.ID_Dish = dm.ID_Dish
						    LEFT JOIN Menus m ON dm.ID_Menu = m.ID_Menu
                            WHERE d.Name LIKE @Name";
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
                throw e;
            }            

            //return (default, default);
                         
        }



                                











        // VERSIONE 0 

        //public async Task<int> UpdateDish(int id, Dish dish)
        //{
        //    string query = "UPDATE Dishes SET Name = @Name, Description = @Description, Price = @Price WHERE ID_Dish = @Id";

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
        //        {
        //            await connection.OpenAsync();
        //            using (SqlCommand command = connection.CreateCommand())
        //            {
        //                using (SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync())
        //                {
        //                    command.Transaction = transaction;

        //                    try
        //                    {
        //                        command.CommandText = query;
        //                        command.Parameters.AddWithValue("@Id", id);
        //                        command.Parameters.AddWithValue("@Name", dish.Name);
        //                        command.Parameters.AddWithValue("@Description", dish.Description);
        //                        command.Parameters.AddWithValue("@Price", dish.Price);
        //                        int rowsAffected = await command.ExecuteNonQueryAsync();



        //                        return rowsAffected;
        //                    }

        //                    catch (Exception e)
        //                    {
        //                        Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
        //                        await transaction.RollbackAsync();
        //                        throw new Exception(e.Message, e.InnerException);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
        //    }

        //    return default;
        //}










        public async Task<int> DeleteDish(int id)
        {

            string query = "DELETE Dishes WHERE ID_Dish = @Id";
            
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

                                await transaction.CommitAsync();

                                return await command.ExecuteNonQueryAsync();
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
                throw e;
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







        public async Task<(int affectedRows,int addedMenuIds,int deletedMenuIds )> UpdateDish2(int id, Dish dish)
        {
            string query = "UPDATE Dishes SET Name = @Name, Description = @Description, Price = @Price, ID_Category = @ID_Category WHERE ID_Dish = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            int rowsAffected = 0;

                            
                            Dictionary<int, Dish> dishes = new Dictionary<int, Dish>();
                            using (SqlCommand readCommand = new SqlCommand("SELECT d.*, m.ID_Menu, m.Name as MenuName, c.Name as CategoryName FROM Dishes d LEFT JOIN Dish_Menu dm ON d.ID_Dish = dm.ID_Dish LEFT JOIN Categories c ON c.ID_Category = d.ID_Category LEFT JOIN Menus m ON dm.ID_Menu = m.ID_Menu WHERE d.ID_Dish = @ID_Dish", connection, transaction))
                            {
                                readCommand.Parameters.AddWithValue("@ID_Dish", id);
                                using (SqlDataReader reader = await readCommand.ExecuteReaderAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        ReadDish(reader, dishes);
                                    }
                                }
                            }

                            Dish toUpdateDish = dishes.Values.FirstOrDefault();
                            if (toUpdateDish == null)
                            {
                                return default;
                            }
                          
                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                
                                command.Parameters.AddWithValue("@Id", id);
                                command.Parameters.AddWithValue("@Name", dish.Name);
                                command.Parameters.AddWithValue("@Description", dish.Description ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@Price", dish.Price);
                                command.Parameters.AddWithValue("@ID_Category", dish.ID_Category);
                                rowsAffected = await command.ExecuteNonQueryAsync();
                            }

                            
                           (int addedMenuIds, int deletedMenuIds) tuplaRowsBridgeTable = await UpdateDishMenus(id, toUpdateDish.MenuIDs, dish.MenuIDs, connection, transaction);


                            await transaction.CommitAsync();

                            
                            return (rowsAffected, tuplaRowsBridgeTable.addedMenuIds , tuplaRowsBridgeTable.deletedMenuIds);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
                            await transaction.RollbackAsync();
                            throw;
                        }

                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.InnerException?.Message}");
                throw e;
            }

            return (default, default, default);
        }


        private async Task<(int addedRows,int deletedRows)> UpdateDishMenus(int dishId, List<int> toUpdateMenuIds, List<int> newMenuIds, SqlConnection connection, SqlTransaction transaction)
        {
            int updatedRows = 0;
            int deletedRows = 0;
            List<int> menusToRemove = toUpdateMenuIds.Except(newMenuIds).ToList();
            if (menusToRemove.Any())
            {
                string deleteQuery = "DELETE FROM Dish_Menu WHERE ID_Dish = @ID_Dish AND ID_Menu = @ID_Menu";

                foreach (int menuId in menusToRemove)
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ID_Dish", dishId);
                        command.Parameters.AddWithValue("@ID_Menu", menuId);
                        deletedRows += await command.ExecuteNonQueryAsync();
                    }
                }

            }


            List<int> menusToAdd = newMenuIds.Except(toUpdateMenuIds).ToList();
            foreach (int menuId in menusToAdd)
            {
                
                string insertQuery = "INSERT INTO Dish_Menu (ID_Dish, ID_Menu) VALUES (@ID_Dish, @ID_Menu)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@ID_Dish", dishId);
                    command.Parameters.AddWithValue("@ID_Menu", menuId);
                    updatedRows += await command.ExecuteNonQueryAsync();
                }
            }

            

            return (updatedRows, deletedRows);
        }

    }       
}


