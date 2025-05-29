using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Models
{
    public class RecipeIngredient
    {
        public Product Product { get; set; } = new Product();
        public double Quantity { get; set; }
        public double TotalCalories => Product.CaloriesPerGram * Quantity;
    }
}
