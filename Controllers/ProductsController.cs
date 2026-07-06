using Microsoft.AspNetCore.Mvc;
using mini_store.Models; // call model
using mini_store.Data;  // call database

namespace mini_store.Controllers
{
    public class ProductsController : Controller
    {
        //***************************************************************************
        private readonly AppDbContext _context; 

        public ProductsController(AppDbContext cn)  //يسوي طبقة جديدة كل مرة نستدعيها
        {
            _context = cn;
        }
        //***************************************************************************

        [Route("dashboard")]
        public IActionResult Index()
        {
            //same meaning as select * from products
            var Products = _context.products.ToList(); //قراءة كل المنتجات في صفحة الانديكس
            var category = _context.categories.ToList();
            ViewBag.categories = category;
            return View(Products); //عرض المنتجات في صفحة العرض
        }

        public IActionResult Delete(int id)
        {
            var pro = _context.products.Find(id);

            if(pro != null)
            {
                _context.products.Remove(pro);
                _context.SaveChanges();
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pro = _context.products.Find(id);
            var category = _context.categories.ToList();
            ViewBag.categories = category;
            if(pro == null)
            {
                return NotFound();
            }
            
            return View(pro);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Create()
        {
            return View();
        }

        //***************************************************************************
        [HttpPost]
        //راح نبني نموذج نخليه يتخزن في ملف product.cs
        public IActionResult Create(Product product) //اسم الكلاس 
        {
            _context.products.Add(product); //اضافة المنتج الى قاعدة البيانات في جدول البرودكت
            _context.SaveChanges(); //حفظ التغييرات 
            return RedirectToAction("Index"); //بعد ما يضيف المنتج يرجع الى صفحة الانديكس
        }
    }
}