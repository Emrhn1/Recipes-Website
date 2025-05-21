using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [Required]
        public string Amount { get; set; }

        public string Unit { get; set; }
    }
} 