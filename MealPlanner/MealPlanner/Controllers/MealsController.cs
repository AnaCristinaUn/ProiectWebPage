using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.AspNetCore.Authorization;

namespace MealPlanner.Controllers
{
    public class MealsController : Controller
    {
        private readonly MealPlannerContext _context;

        public MealsController(MealPlannerContext context)
        {
            _context = context;
        }

        // GET: Meals
        public async Task<IActionResult> Index(string mealIngredients, string searchString)
        {
            if (_context.Meal == null)
            {
                return Problem("Entity set 'MealPlannerContext.Meal'  is null.");
            }

            var mealsQuery = _context.Meal
                .Include(m => m.Ingredients)
                .OrderByDescending(m => m.AddedDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                mealsQuery = mealsQuery.Where(m => m.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(mealIngredients))
            {
                mealsQuery = mealsQuery.Where(m => m.Ingredients.Any(i => i.Name == mealIngredients));
            }

            var ingredientsList = await _context.Ingredient
                .Select(i => i.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();

            var mealIngredientsVM = new MealIngredientsViewModel
            {
                Ingredients = new SelectList(ingredientsList),
                Meals = await mealsQuery.ToListAsync()
            };

            return View(mealIngredientsVM);
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            ViewData["MealTypeList"] = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>());
            return View();
        }

        // POST: Meals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddedDate,MealType,IngredientsInput,CookingTime,Rating")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(meal.IngredientsInput))
                {
                    var ingredientNames = meal.IngredientsInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => new Ingredient { Name = name.Trim() })
                        .ToList();

                    meal.Ingredients = ingredientNames;
                }

                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MealTypeList"] = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>(), meal.MealType);
            return View(meal);
        }

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meal == null)
            {
                return NotFound();
            }

            meal.IngredientsInput = string.Join(", ", meal.Ingredients.Select(i => i.Name));
            ViewData["MealTypeList"] = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>(), meal.MealType);
            return View(meal);
        }

        // POST: Meals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AddedDate,MealType,IngredientsInput,CookingTime,Rating")] Meal meal)
        {
            if (id != meal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mealToUpdate = await _context.Meal
                        .Include(m => m.Ingredients)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (mealToUpdate == null)
                        return NotFound();

                    mealToUpdate.Name = meal.Name;
                    mealToUpdate.AddedDate = meal.AddedDate;
                    mealToUpdate.MealType = meal.MealType;
                    mealToUpdate.CookingTime = meal.CookingTime;
                    mealToUpdate.Rating = meal.Rating;

                    _context.Ingredient.RemoveRange(mealToUpdate.Ingredients);

                    if (!string.IsNullOrWhiteSpace(meal.IngredientsInput))
                    {
                        mealToUpdate.Ingredients = meal.IngredientsInput
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(name => new Ingredient { Name = name.Trim(), MealId = meal.Id })
                            .ToList();
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.Id))
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

            ViewData["MealTypeList"] = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>(), meal.MealType);
            return View(meal);
        }

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meal.FindAsync(id);
            if (meal != null)
            {
                _context.Meal.Remove(meal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.Id == id);
        }
    }
}
