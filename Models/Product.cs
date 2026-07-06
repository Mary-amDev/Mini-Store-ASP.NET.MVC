using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Product
    {
 
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; } //Foriegn Key

        [ForeignKey("CategoryId")]
        public virtual Categories category{get; set;} //مفتاح اجنبي ينتمي الى الكاتيقوريز و يشوف كل المتغيرات
        public virtual ICollection <Image> Images {get; set;} = new List<Image>();



    }
}