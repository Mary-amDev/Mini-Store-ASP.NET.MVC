using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Image
    {
 
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsId {get; set;}

        [ForeignKey("ProductsId")]
        public virtual Product Product {get; set;}


}   }
