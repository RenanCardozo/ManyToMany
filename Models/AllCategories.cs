#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductCategory.Models;

public class AllCategories
{
    public Category? Category { get; set; }
    public List<Category>? Categories { get; set; }
}