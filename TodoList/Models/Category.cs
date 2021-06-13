using TodoList.UniqueAttribute;
using System.ComponentModel.DataAnnotations;


namespace TodoList.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Unique(ErrorMessage = "Category already exist !!")]
        public string Name { get; set; }
    }
}