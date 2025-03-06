using System.ComponentModel.DataAnnotations;

namespace ristorante_backend.Models
{
    public class User
    {
        public int ID_User { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
    public class UserModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
