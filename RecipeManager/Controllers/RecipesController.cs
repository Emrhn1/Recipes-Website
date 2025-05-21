using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Controllers
{
        public class RecipesController : Controller    {        private readonly ApplicationDbContext _context;        private readonly ILogger<RecipesController> _logger;        public RecipesController(ApplicationDbContext context, ILogger<RecipesController> logger)        {            _context = context;            _logger = logger;        }

        // GET: Recipes
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            const int pageSize = 10;
            var recipes = from r in _context.Recipes
                         select r;

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    recipes = recipes.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    recipes = recipes.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    recipes = recipes.OrderByDescending(s => s.CreatedDate);
                    break;
                default:
                    recipes = recipes.OrderBy(s => s.Title);
                    break;
            }

            return View(await PaginatedList<Recipe>.CreateAsync(recipes.Include(r => r.Category), pageNumber ?? 1, pageSize));
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("Title,Description,Instructions,PrepTime,CookTime,CategoryId")] Recipe recipe)        {            try            {                if (ModelState.IsValid)                {                    recipe.CreatedDate = DateTime.Now;                    recipe.RecipeIngredients = new List<RecipeIngredient>();                    var category = await _context.Categories.FindAsync(recipe.CategoryId);                    if (category == null)                    {                        ModelState.AddModelError("CategoryId", "Selected category does not exist.");                        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);                        return View(recipe);                    }                    recipe.Category = category;                    _context.Add(recipe);                    await _context.SaveChangesAsync();                    return RedirectToAction(nameof(Index));                }                else                {                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))                    {                        _logger.LogError($"Model Error: {modelError.ErrorMessage}");                    }                }            }            catch (Exception ex)            {                _logger.LogError($"Error creating recipe: {ex.Message}");                ModelState.AddModelError("", "An error occurred while saving the recipe. Please try again.");            }            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Instructions,PrepTime,CookTime,CategoryId,CreatedDate")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
} 