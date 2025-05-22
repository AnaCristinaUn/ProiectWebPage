using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Models;

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner
}

public class Meal
{
    public int Id { get; set; }

    [StringLength(80, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public DateTime AddedDate { get; set; }

    [Required]
    public MealType MealType { get; set; }

    [Display(Name = "Cooking Time (minutes)")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public decimal CookingTime { get; set; }

    [Range(1,5)]
    public double Rating { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new();

    [NotMapped]
    public string IngredientsInput { get; set; } = "";

}
