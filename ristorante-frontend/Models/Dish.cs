using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ristorante_frontend.Models;

namespace ristorante_frontend.NewFolder
{
    public class Dish
    {
        public int ID_Dish { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Category? Category { get; set; }
    }
}
