#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductCategory.Models;

public class AllProducts
{
    public Product? Product { get; set; }
    public List<Product>? Products { get; set; }
}