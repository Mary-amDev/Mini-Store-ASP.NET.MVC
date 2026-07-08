using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mini_store.ViewModels;
using System.Threading.Tasks;

namespace mini_store.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

         private readonly UserManager<IdentityUser> _userManager; // أضفنا هذا السطر

        // 2. حقن الأدوات في المُشيّد (Constructor)
        public AccountController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager) 
        {
            _signInManager = signInManager;
            _userManager = userManager; 
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
    // تنظيف جلسة الدخول وحذف ملف تعريف الارتباط (Cookie)
    await _signInManager.SignOutAsync();
    
    // توجيه المستخدم إلى الصفحة الرئيسية بعد تسجيل الخروج
    return RedirectToAction("Index", "Home");
}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // محاولة تسجيل الدخول
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // إذا كان هناك رابط سابق، أعده إليه (مثل لوحة التحكم)، وإلا خذه للصفحة الرئيسية
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                
                // إذا كانت البيانات خاطئة
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }

            return View(model);
        }

        [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            // هذه الدالة مخصصة لاستقبال البيانات من النموذج وحفظها
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Register(RegisterViewModel model)
        // {
        //     // 1. التحقق من أن المدخلات تطابق الشروط (مثل تطابق كلمتي المرور وصحة البريد)
        //     if (ModelState.IsValid)
        //     {
        //         // 2. إنشاء كائن مستخدم جديد وتعبئة بياناته
        //         var user = new IdentityUser 
        //         { 
        //             UserName = model.Email, 
        //             Email = model.Email 
        //         };
                
        //         // 3. حفظ المستخدم في قاعدة البيانات وتشفير كلمة المرور تلقائياً
        //         var result = await _userManager.CreateAsync(user, model.Password);

        //         // 4. إذا تم الحفظ بنجاح
        //         if (result.Succeeded)
        //         {
        //             // تسجيل دخول المستخدم الجديد تلقائياً
        //             await _signInManager.SignInAsync(user, isPersistent: false);
                    
        //             // توجيهه إلى الصفحة الرئيسية للمتجر (أو أي صفحة تريدها)
        //             return RedirectToAction("Index", "Home");
        //         }

        //         // 5. إذا فشل الحفظ (مثلاً: الإيميل مستخدم مسبقاً، أو كلمة المرور ضعيفة)
        //         // نقوم بإضافة الأخطاء لعرضها للمستخدم في الصفحة
        //         foreach (var error in result.Errors)
        //         {
        //             ModelState.AddModelError(string.Empty, error.Description);
        //         }
        //     }

        //     // إذا كانت هناك أخطاء في المدخلات، نعيد عرض الصفحة مع الأخطاء
        //     return View(model);
        // }
    }


    
}