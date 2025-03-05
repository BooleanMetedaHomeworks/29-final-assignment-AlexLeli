using Microsoft.Data.SqlClient;
using ristorante_backend.Models;

namespace ristorante_backend.Repositories
{
    public class MenuRepository
    {

        private const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=GestionaleRistorante;Integrated Security=True;Trust Server Certificate=True";

        public async Task<List<Menu>> GetAllMenus()
            {
                string query = "SELECT * FROM Menus";
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();

                List<Menu> menus = new List<Menu>();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            menus.Add(GetMenuFromData(reader));
                        }
                    }
                }

                return menus;
            }

            public async Task<List<Menu>> GetMenuByName(string name)
            {
                string query = "SELECT * FROM Menus WHERE Name LIKE @Name";
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();

                List<Menu> menus = new List<Menu>();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", $"%{name}%");

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            menus.Add(GetMenuFromData(reader));
                        }
                    }
                }

                return menus;
            }

            public async Task<Menu> GetMenuById(int id)
            {
                var query = "SELECT * FROM Menus WHERE ID_Menu = @ID_Menu";
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID_Menu", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return GetMenuFromData(reader);
                        }
                    }
                }

                return null;

            }

            public async Task<int> CreateMenu(Menu menu)
            { 
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();

                var query = $"INSERT INTO Menus (Name) VALUES (@Name);" +
                    $"SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Name", menu.Name));

                    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }

            public async Task<int> UpdateMenu(int id, Menu menu)
            {
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();

                var query = $"UPDATE Menus SET Name = @Name WHERE ID_Menu = @ID_Menu;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ID_Menu", id));
                    cmd.Parameters.Add(new SqlParameter("@Name", menu.Name));

                    return await cmd.ExecuteNonQueryAsync();
                }
            }

            public async Task<int> DeleteMenu(int id)
            {

                string query = "DELETE FROM Menus WHERE ID_Menu = @ID_Menu";
                using SqlConnection conn = new SqlConnection(CONNECTION_STRING);
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID_Menu", id);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }

            private Menu GetMenuFromData(SqlDataReader reader)
            {
                int id = reader.GetInt32(reader.GetOrdinal("Id_Menu"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                Menu menu = new Menu(id, name);
                return menu;
            }
        }
    }



