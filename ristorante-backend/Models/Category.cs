using System.ComponentModel.DataAnnotations;

namespace ristorante_backend.Models
{
    public class Category
    {
        public int ID_Category { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Name { get; set; }

        public Category() { }

        public Category(int id, string name)
        {
            ID_Category = id;
            Name = name;
        }
    }
}
