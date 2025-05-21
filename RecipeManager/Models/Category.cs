using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class Category
    {
        public Category()
        {
            Recipes = new List<Recipe>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Recipe>? Recipes { get; set; }
    }
} 