using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Models;

public class Meal
{
    public int Id { get; set; }
    [StringLength(80, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }
    [Display(Name = "Date Added")]
    [DataType(DataType.Date)]
    public DateTime AddedDate { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9\s,]*$")]
    [Required]
    public string? Ingredients { get; set; }
    [Display(Name = "Cooking Time (minutes)")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public decimal CookingTime { get; set; }
    [Range(1,5)]
    public double Rating { get; set; }
    
}
