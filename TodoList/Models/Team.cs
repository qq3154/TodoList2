using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TodoList.UniqueAttribute;

namespace TodoList.Models
{
	public class Team
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		[Unique(ErrorMessage = "Team Name already exist !!")]
		[DisplayName("Team Name")]
		public string Name { get; set; }
	}
}