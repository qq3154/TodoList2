using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoList.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } // Link Object
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}