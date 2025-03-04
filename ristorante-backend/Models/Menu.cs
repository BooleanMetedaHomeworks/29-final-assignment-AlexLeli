using System.ComponentModel.DataAnnotations;

namespace ristorante_backend.Models
{
    public class Menu
    {
        public int ID_Menu { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Name { get; set; }

        public Menu() { }

        public Menu(int id, string name)
        {
            ID_Menu = id;
            Name = name;
        }
    }
}
