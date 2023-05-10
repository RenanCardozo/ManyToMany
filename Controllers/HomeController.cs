using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductCategory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace ProductCategory.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    //////////////////?View Pages?////////////////////////
    // Product List and form
    [HttpGet("/")]
    public IActionResult Index()
    {
        AllProducts allProducts = new AllProducts
        {
            Products = _context.Products.ToList()
        };
        return View("Index", allProducts);
    }

    //Category List and form
    [HttpGet("/categories")]
    public IActionResult Categories()
    {
        AllCategories allCategories = new AllCategories
        {
            Categories = _context.Categories.ToList()
        };
        return View("CategoryCreateView", allCategories);
    }

    //product view page that has the categories to choose from
    [HttpGet("/product/{productId}")]
    public IActionResult ProductAsso(int productId)
    {
        Product? product = _context.Products.Include(p => p.Associations).ThenInclude(a => a.Category).FirstOrDefault(p => p.ProductId == productId);

        List<Category> allCategories = _context.Categories.ToList();
        List<Category> availableCategories = allCategories.Where(c => !product.Associations.Any(a => a.CategoryId == c.CategoryId)).ToList();

        ProductCategoryViewModel viewModel = new ProductCategoryViewModel
        {
            Product = product,
            Categories = _context.Categories.ToList(),
            AvailableCategories = availableCategories
        };
        return View("ProductAssociation", viewModel);
    }

    //category view page that has the products to choose from
    [HttpGet("/category/{categoryId}")]
    public IActionResult CategoryAsso(int categoryId)
    {
        Category? category = _context.Categories.Include(c => c.Associations).ThenInclude(a => a.Product).FirstOrDefault(c => c.CategoryId == categoryId);

        List<Product> allProducts = _context.Products.ToList();
        List<Product> availableProducts = allProducts.Where(p => !category.Associations.Any(a => a.ProductId == p.ProductId)).ToList();

        ProductCategoryViewModel viewModel = new ProductCategoryViewModel
        {
            Category = category,
            Products = _context.Products.ToList(),
            AvailableProducts = availableProducts
        };

        return View("CategoryAssociation", viewModel);
    }



    //////////////!Create!/////////////////////

    //creates a new product
    [HttpPost("createProduct")]
    public IActionResult CreateProduct(AllProducts products)
    {
        if (ModelState.IsValid && products.Product != null)
        {
            _context.Products.Add(products.Product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        products.Products = _context.Products.ToList();
        return View("Index", products);
    }

    //created a category
    [HttpPost("createCategory")]
    public IActionResult CreateCategory(AllCategories allCategories)
    {
        if (ModelState.IsValid && allCategories.Category != null)
        {
            _context.Categories.Add(allCategories.Category);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }


        allCategories.Categories = _context.Categories.ToList();
        return View("Categories", allCategories);
    }


    [HttpPost("product/association/create/{productId}")]
    public IActionResult CreateProductAssociation(int productId, Association association)
    {
        if (ModelState.IsValid && association != null)
        {
            association.ProductId = productId; 
            _context.Associations.Add(association);
            _context.SaveChanges();
            return RedirectToAction("ProductAsso", new { productId = association.ProductId });
        }

        Product? product = _context.Products.Include(p => p.Associations).ThenInclude(a => a.Category).FirstOrDefault(p => p.ProductId == productId);

        List<Category> allCategories = _context.Categories.ToList();
        List<Category> availableCategories = allCategories.Where(c => !product.Associations.Any(a => a.CategoryId == c.CategoryId)).ToList();

        ProductCategoryViewModel updatedViewModel = new ProductCategoryViewModel
        {
            Product = product,
            Categories = _context.Categories.ToList(),
            AvailableCategories = availableCategories,
            Association = association
        };

        return View("ProductAssociation", updatedViewModel);
    }


    [HttpPost("category/association/create/{categoryId}")]
    public IActionResult CreateCategoryAssociation(int categoryId, Association association)
    {
        if (ModelState.IsValid && association != null)
        {
            association.CategoryId = categoryId; // Set the CategoryId property
            _context.Associations.Add(association);
            _context.SaveChanges();
            return RedirectToAction("CategoryAsso", new { categoryId = association.CategoryId });
        }

        Category? category = _context.Categories.Include(c => c.Associations).ThenInclude(a => a.Product).FirstOrDefault(c => c.CategoryId == categoryId);

        List<Product> allProducts = _context.Products.ToList();
        List<Product> availableProducts = allProducts.Where(p => !category.Associations.Any(a => a.ProductId == p.ProductId)).ToList();

        ProductCategoryViewModel updatedViewModel = new ProductCategoryViewModel
        {
            Category = category,
            Products = _context.Products.ToList(),
            AvailableProducts = availableProducts,
            Association = association
        };

        return View("CategoryAssociation", updatedViewModel);
    }

    //////////////?       Delete     ?////////////////////

    [HttpGet("deleteAssociation/{pagefrom}/{associationId}")]
    public IActionResult DeleteAssociation(string pagefrom,int associationId)
    {
        if(pagefrom != "Products" && pagefrom != "Categories"){
            return RedirectToAction("Index");
        }
        Association? association = _context.Associations.FirstOrDefault(a => a.AssociationId == associationId);

        _context.Associations.Remove(association);
        _context.SaveChanges();
        if(pagefrom== "Products"){
        int productId = association.ProductId;
        return RedirectToAction("ProductAsso", new { productId });
        }
        else
        {
            int categoryId = association.CategoryId;
            return RedirectToAction("CategoryAsso", new {categoryId});
        }
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
