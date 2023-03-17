#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductCategory.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage ="Category Name is required")]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Association> Associations { get; set; } = new List<Association>();

}