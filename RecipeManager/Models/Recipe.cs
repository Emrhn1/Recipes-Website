using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new List<RecipeIngredient>();
            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Instructions { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Preparation time must be a positive number")]
        public int PrepTime { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Cooking time must be a positive number")]
        public int CookTime { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
    }
} 