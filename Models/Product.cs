using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "عذرا ً, يجب ادخال اسم المنتج")]
        [StringLength(100, ErrorMessage ="اسم المنتج طويل جداً, الحد الأقصى 100 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى تحديد سعر المنتج")]
        [Range(0.01 , 10500.00 , ErrorMessage ="الحد الأدنى للسعر 0.0.1 و الحد الأقصى 10500")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "رابط الصورة مطلوب")]
        public string Image { get; set; }
        public int CategoryId { get; set; } //Foriegn Key
        
        [NotMapped]
        [Required(ErrorMessage ="يجب اختيار صورة")]
        public IFormFile? ImageFile {get ; set ;}
        
        [ForeignKey("CategoryId")]
        public virtual Categories? category{get; set;} //مفتاح اجنبي ينتمي الى الكاتيقوريز و يشوف كل المتغيرات
        



    }
}