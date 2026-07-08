using Microsoft.AspNetCore.Mvc;
using mini_store.Models; // call model
using mini_store.Data;  // call database
using Microsoft.AspNetCore.Authorization;

namespace mini_store.Controllers
{
    public class ProductsController : Controller
    {
        //***************************************************************************
        private readonly AppDbContext _context; 

        public readonly IWebHostEnvironment _webhostingEnviroment; //ترفع الصور في مجلد الصور

        public ProductsController(AppDbContext cn , IWebHostEnvironment webhost)  //يسوي طبقة جديدة كل مرة نستدعيها
        {
            _context = cn;
            _webhostingEnviroment = webhost;
        }
        //***************************************************************************

        [Route("dashboard")]
        [Authorize]
        public IActionResult Index(string searchTerm)
        {
            //same meaning as select * from products
            //var Products = _context.products.ToList(); //قراءة كل المنتجات في صفحة الانديكس
            var category = _context.categories.ToList();
            ViewBag.categories = category;
            var Productquery=_context.products.AsQueryable();

            if(!string.IsNullOrEmpty(searchTerm))
            {
                Productquery=Productquery.Where(p=>p.Name.Contains(searchTerm));
            }

            
            var products = Productquery.ToList();

            ViewBag.CurrentSearch = searchTerm;
            return View(products); //عرض المنتجات في صفحة العرض
        }

        //****************************************************************************

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //راح نبني نموذج نخليه يتخزن في ملف product.cs
        public IActionResult Create(Product product) //اسم الكلاس 
        {
            if(product.ImageFile != null)
            {
                //مسار حفظ الصور
                string UploadFolder = Path.Combine(_webhostingEnviroment.WebRootPath,"images");
                if(!Directory.Exists(UploadFolder))// اذا الصورة المخزنة داخل المجلد غير مونشأ
                {
                    Directory.CreateDirectory(UploadFolder); //نقول له ينشئه
                }

                string extention = Path.GetExtension(product.ImageFile.FileName);
                //نعطيه اسم مميز عشان الملف/الصورة اللي بنفس الاسم ما تحذف القديمة
                //guid تعطي قيم عشوائية
                string UniqueFileName = Guid.NewGuid().ToString() + extention;
                string Filepath = Path.Combine(UploadFolder,UniqueFileName); 

                product.Image = UniqueFileName;
                ModelState.Remove(nameof(Product.Image));

                using(var fileStreame = new FileStream(Filepath,FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStreame);
                }
            }

            if(ModelState.IsValid)
            {
                _context.products.Add(product); //اضافة المنتج الى قاعدة البيانات في جدول البرودكت
                _context.SaveChanges(); //حفظ التغييرات 

                return RedirectToAction("Index"); //بعد ما يضيف المنتج يرجع الى صفحة الانديكس
            }
            ViewBag.categories = _context.categories.ToList();
            return View("Index" , _context.products.ToList());

        }

        
        //****************************************************************************
        public IActionResult Delete(int id)
        {
            var pro = _context.products.Find(id);

            if(pro != null)
            {
                _context.products.Remove(pro);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //****************************************************************************
        
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

            return RedirectToAction("Index");
        }


    }
}