using Microsoft.AspNetCore.Mvc;
using mini_store.Models; // call model
using mini_store.Data;

namespace mini_store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context; 

        public CategoriesController(AppDbContext cn)  //يسوي رفرش (دالة بناء)
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

        [HttpPost]
        public IActionResult Create(Categories category)
        {
            _context.categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //***************************************************************************
   
    }
}