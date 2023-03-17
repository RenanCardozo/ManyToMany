#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductCategory.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage ="Product Name is Required")]
    public string Name { get; set; }

    [Required(ErrorMessage ="Product Description is Required")]
    public string Description { get; set; }

    [Required(ErrorMessage ="Price Is Required")]
    public float Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Association> Associations { get; set; } = new List<Association>();

}