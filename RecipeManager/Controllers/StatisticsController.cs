using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models.ViewModels;

namespace RecipeManager.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var recipesCount = await _context.Recipes.CountAsync();
            
            var statistics = new StatisticsViewModel
            {
                TotalRecipes = recipesCount,
                TotalCategories = await _context.Categories.CountAsync(),
                TotalIngredients = await _context.Ingredients.CountAsync(),
                AveragePrepTime = recipesCount > 0 ? await _context.Recipes.AverageAsync(r => r.PrepTime) : 0,
                AverageCookTime = recipesCount > 0 ? await _context.Recipes.AverageAsync(r => r.CookTime) : 0,
                RecipesPerCategory = await _context.Categories
                    .Select(c => new CategoryStatistics
                    {
                        CategoryName = c.Name,
                        RecipeCount = c.Recipes != null ? c.Recipes.Count : 0
                    })
                    .ToListAsync(),
                MostUsedIngredients = await _context.RecipeIngredients
                    .GroupBy(ri => ri.Ingredient.Name)
                    .Select(g => new IngredientStatistics
                    {
                        IngredientName = g.Key,
                        UsageCount = g.Count()
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .Take(5)
                    .ToListAsync()
            };

            return View(statistics);
        }
    }
} 