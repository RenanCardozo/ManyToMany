#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductCategory.Models;

public class ProductCategoryViewModel
{
    public Association? Association { get; set; }
    public List<Association>? Associations { get; set; }

    public Category? Category { get; set; }
    public List<Category>? Categories { get; set; }
    
    public Product? Product { get; set; }
    public List<Product>? Products { get; set; }

    public List<Category>? AvailableCategories { get; set; }
    public List<Product>? AvailableProducts { get; set; }
}