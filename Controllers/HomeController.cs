using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mini_store.Models;
using mini_store.Data;

namespace mini_store.Controllers;

public class HomeController : Controller
{



    //____________________________________________________________________________
    //عرفناهم في البداية او خارج الدوال عشان تكون عامة مرئية لجميع الدوال
    private static dynamic[] _categories =
    {
       new { Id = 0, Name = "إلكترونيات", Icon = "fa -solid fa-bolt-lightning" }, 
       new { Id = 1, Name = "ملابس", Icon = "fa-solid fa-shirt" },
       new { Id = 2, Name = "كتب", Icon = "fas fa-book-open" }
    };

    private static dynamic[] _products =
    {   //categoryId is the forien key to the categories table.
      new { categoryId = 0, Name = "هاتف ذكي", Price = 2500, Description = "هاتف ذكي بكاميرا عالية الدقة", Image = "phone.jpg" },  
      new { categoryId = 0, Name = "حاسوب محمول", Price = 4500, Description = "حاسوب مخصص للمطورين", Image = "laptop.jpg" },  
      new { categoryId = 1, Name = "قميص قطني", Price = 150, Description = "قميص مريح و صيفي", Image = "shirt.jpg" },  
      new { categoryId = 2, Name = "كتاب برمجة", Price = 90, Description = "دليل شامل لتعلم البرمجة", Image = "book.jpg" }  
    };
    //____________________________________________________________________________

//***************************************************************************
    private readonly AppDbContext _context; 

    public HomeController(AppDbContext cn)  //يسوي رفرش (دالة بناء)
    {
        _context = cn;
    }
//***************************************************************************
    public IActionResult Index()
    {
        //same meaning as select * from products
        var Category = _context.categories.ToList(); //قراءة كل المنتجات في صفحة الانديكس   
        return View(Category); 
    }

    [Route("List")]
    public IActionResult Products (int id)
    { //id يجي من اليوزر
        var filtered = _products
            .Where(p => p.categoryId == id) .ToList(); //حيقرأ مصفوفة البرودكت سطر سطر و بعدها يشوف الشرط  
           
         ViewBag.FilteredProducts = filtered;
         ViewBag.categoryName = _categories[id];
        return View();
    }

    public IActionResult Details(string name)
    {
        //نستخدم FirstOrDefault عشان يجيب سجل واحد فقط = قيمة واحدة
        var product = _products.FirstOrDefault(p => p.Name == name);
        if(product == null)
        {
            return NotFound();
        }
        ViewBag.Product = product;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
