using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TodoList.Models;

namespace TodoList.ViewModels
{
    public class TodoCategoriesViewModel
    {
        public Todo Todo { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}