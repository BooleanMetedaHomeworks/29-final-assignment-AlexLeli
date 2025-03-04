using System.ComponentModel.DataAnnotations;

namespace ristorante_backend.Models
{
    public class Dish
    {
        public int ID_Dish { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Name { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Il nome non può superare i 255 caratteri")]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Chiave esterna (può essere NULL)
        public int? ID_Category { get; set; }
        public Category? Category { get; set; }

        public List<int> MenuIDs { get; set; } = new List<int>();
        public List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
