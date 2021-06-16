using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TodoList.Models;

namespace TodoList.ViewModels
{
    public class TeamUsersViewModel
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}