using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Models;

public class Ingredient
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    // Legătura către Meal
    public int MealId { get; set; }
    [ForeignKey("MealId")]
    public Meal Meal { get; set; } = null!;
}
