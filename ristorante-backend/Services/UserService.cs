using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using ristorante_backend.Models;

namespace ristorante_backend.Services
{
    public class UserService
    {
        private const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=GestionaleRistorante;Integrated Security=True;Trust Server Certificate=True";

        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public UserService(IPasswordHasher<UserModel> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterAsync(UserModel User)
        {
            using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            string ricercaUser = "SELECT * FROM Users where Email = @Email";
            using (SqlCommand commandRicerca = new SqlCommand(ricercaUser, connection))
            {
                commandRicerca.Parameters.AddWithValue("@Email", User.Email);
                SqlDataReader reader = await commandRicerca.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    throw (new Exception(message: "Esiste già un User registrato con l' email inserita"));
            }

         

            string passwordHash = _passwordHasher.HashPassword(User, User.Password);

            string query = "INSERT INTO Users (Email, PasswordHash) VALUES (@Email, @PasswordHash)";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            return await command.ExecuteNonQueryAsync() > 0;

        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue($"@Email", email);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID_User"));
                string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                UserModel model = new UserModel() { Email = email, Password = password };
                if (_passwordHasher.VerifyHashedPassword(model, passwordHash, password) != PasswordVerificationResult.Success)
                {
                    return null;
                }
                return new User() { ID_User = id, Email = email };
            }
            return null;
        }


        public async Task<List<string>> GetUserRolesAsync(int UserId)
        {
            List<string> ruoli = new List<string>();
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(
                "SELECT r.Name " +
                "FROM Roles r " +
                "INNER JOIN UserRole ur ON r.ID_User = ur.ID_User " +
                "WHERE ur.ID_User = @ID_User", connection);
                command.Parameters.AddWithValue("@ID_User", UserId);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ruoli.Add(reader.GetString(0));
                }
            }
            return ruoli;
        }

        public async Task<User> GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string query = "select * from Users where ID_User = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    User u = new User();
                    u.ID_User = reader.GetInt32(reader.GetOrdinal("ID_User"));
                    u.Email = reader.GetString(reader.GetOrdinal("Email"));
                    return u;
                }
            }
            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string query = "select * from Users where Email = @email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    User u = new User();
                    u.ID_User = reader.GetInt32(reader.GetOrdinal("ID_User"));
                    u.Email = reader.GetString(reader.GetOrdinal("Email"));
                    return u;
                }
            }
            return null;
        }

    }

}
