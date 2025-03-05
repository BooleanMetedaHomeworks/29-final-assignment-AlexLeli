using Microsoft.Data.SqlClient;
using ristorante_backend.Models;

namespace ristorante_backend.Repositories
{
    public class CategoryRepository
    {
        private const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=GestionaleRistorante;Integrated Security=True;Trust Server Certificate=True";
       
        public async Task<List<Category>> GetAllCategories()
        {
            var query = "SELECT * FROM Categories";
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();

            List<Category> categories = new List<Category>();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(GetCategoryFromData(reader));
                    }
                }
            }

            return categories;
        }

        public async Task<List<Category>> GetCategoriesByName(string name)
        {
            var query = "SELECT * FROM Categories WHERE Name LIKE @Name";
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();

            List<Category> categories = new List<Category>();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", $"%{name}%");

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(GetCategoryFromData(reader));
                    }
                }
            }

            return categories;
        }

        public async Task<Category> GetCategoriesById(int id)
        {
            var query = "SELECT * FROM Categories WHERE ID_Category = @ID_Category";
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();

            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID_Category", id);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return GetCategoryFromData(reader);
                    }
                }
            }

            return null;

        }

        public async Task<int> CreateCategory(Category category)
        {
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();

            var query = @"INSERT INTO Categories (Name) VALUES (@Name) 
                          SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Name", category.Name));

                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task<int> UpdateCategory(int id, Category category)
        {
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();

            var query = $"UPDATE Categories SET Name = @Name WHERE ID_Category = @ID_Category;";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ID_Category", id));
                cmd.Parameters.Add(new SqlParameter("@Name", category.Name));

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeleteCategory(int id)
        {

            string query = "DELETE FROM Categories WHERE ID_Category = @ID_Category";
            using var conn = new SqlConnection(CONNECTION_STRING);
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID_Category", id);
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        private Category GetCategoryFromData(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id_Category"));
            string name = reader.GetString(reader.GetOrdinal("Name"));
            Category category = new Category(id, name);
            return category;
        }
    }
}
