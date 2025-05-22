using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MealPlanner.Models;
public class MealIngredientsViewModel
{
    public List<Meal>? Meals { get; set; }
    public SelectList? Ingredients { get; set; }
    public string? MealIngredients { get; set; }
    public string? SearchString { get; set; }

}

