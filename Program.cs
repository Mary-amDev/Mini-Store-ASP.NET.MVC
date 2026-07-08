using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mini_store.Data;
using mini_store; 
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// 1. إعداد مسار ملفات الترجمة
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 2. إعداد قاعدة البيانات
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// 3. دمج MVC مع إعدادات الترجمة
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => 
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// 4. تسجيل إعدادات اللغات في حاوية الخدمات (يتم تعريف المتغير مرة واحدة فقط هنا)
var supportedCultures = new[] { "ar", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0]); 
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    
    // إزالة مزود لغة المتصفح
    var browserLanguageProvider = options.RequestCultureProviders
        .OfType<Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider>()
        .FirstOrDefault();

    if (browserLanguageProvider != null)
    {
        options.RequestCultureProviders.Remove(browserLanguageProvider);
    }
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    // تخفيف شروط كلمات المرور لتسهيل التجربة
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();


// 5. تفعيل البرمجية الوسيطة للغات
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("ar")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// إعداد مسار الطلبات (HTTP request pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();