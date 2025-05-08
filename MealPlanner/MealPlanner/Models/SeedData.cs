using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MealPlanner.Data;
using System;
using System.Linq;

namespace MealPlanner.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MealPlannerContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MealPlannerContext>>()))
        {
            // Look for any movies.
            if (context.Meal.Any())
            {
                return;   // DB has been seeded
            }
            context.Meal.AddRange(
                new Meal
                {
                    Name = "Meal3",
                    AddedDate = DateTime.Parse("1989-2-12"),
                    Ingredients = "ingredients3",
                    CookingTime = 15,
                    Rating = 2
                },
                new Meal
                {
                    Name = "Meal4 ",
                    AddedDate = DateTime.Parse("1984-3-13"),
                    Ingredients = "ingredients4",
                    CookingTime = 35,
                    Rating = 4
                },
                new Meal
                {
                    Name = "Meal5",
                    AddedDate = DateTime.Parse("1986-2-23"),
                    Ingredients = "ingredients5",
                    CookingTime = 20,
                    Rating = 4.5
                },
                new Meal
                {
                    Name = "Meal6",
                    AddedDate = DateTime.Parse("1959-4-15"),
                    Ingredients = "ingredients6",
                    CookingTime = 10,
                    Rating = 3.5
                }
            );
            context.SaveChanges();
        }
    }
}
