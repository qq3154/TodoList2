using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
	[Authorize(Roles = "manager")]
	public class TeamsController : Controller
	{
		private ApplicationDbContext _context;
		public TeamsController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Teams
		[HttpGet]
		public ActionResult Index()
		{
			var teams = _context.Teams.ToList();
			return View(teams);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Team team)
		{
			if (!ModelState.IsValid)
			{
				return View(team);
			}

			var newTeam = new Team
			{
				Name = team.Name
			};

			_context.Teams.Add(newTeam);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}