using System.ComponentModel.DataAnnotations;

namespace RokredBackend.Models
{
    public class Category
    {
        [Key]
        public  string Guid { get; set; }
        
        public string ParentGiud { get; set; }
        
        public bool IsNew { get; set; }
        
        public string Name { get; set; }
    }
}