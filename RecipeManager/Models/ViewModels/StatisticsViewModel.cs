using System.Collections.Generic;

namespace RecipeManager.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalRecipes { get; set; }
        public int TotalCategories { get; set; }
        public int TotalIngredients { get; set; }
        public double AveragePrepTime { get; set; }
        public double AverageCookTime { get; set; }
        public List<CategoryStatistics> RecipesPerCategory { get; set; }
        public List<IngredientStatistics> MostUsedIngredients { get; set; }
    }

    public class CategoryStatistics
    {
        public string CategoryName { get; set; }
        public int RecipeCount { get; set; }
    }

    public class IngredientStatistics
    {
        public string IngredientName { get; set; }
        public int UsageCount { get; set; }
    }
} 