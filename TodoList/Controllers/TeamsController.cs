using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TodoList.ViewModels;

namespace TodoList.Controllers
{
	[Authorize]
	public class TeamsController : Controller
	{
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;
		public TeamsController()
		{
			_context = new ApplicationDbContext();
			_userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		// GET: Teams
		[Authorize(Roles = "manager")]
		[HttpGet]
		public ActionResult Index()
		{
			var teams = _context.Teams.ToList();
			return View(teams);
		}

		[Authorize(Roles = "manager")]
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[Authorize(Roles = "manager")]
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

		[Authorize(Roles = "manager")]
		[HttpGet]
		public ActionResult Members(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			var members = _context.TeamsUsers
				.Include(t => t.User)
				.Where(t => t.TeamId == id)
				.Select(t => t.User);
			ViewBag.TeamId = id;

			return View(members);
		}

		[Authorize(Roles = "manager")]
		[HttpGet]
		public ActionResult AddMembers(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			if (_context.Teams.SingleOrDefault(t => t.Id == id) == null)
				return HttpNotFound();

			var usersInDb = _context.Users.ToList();      // User trong Db

			var usersInTeam = _context.TeamsUsers         // User trong Team
				.Include(t => t.User)
				.Where(t => t.TeamId == id)
				.Select(t => t.User)
				.ToList();

			var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Team

			foreach (var user in usersInDb)
			{
				if (!usersInTeam.Contains(user) &&
					_userManager.GetRoles(user.Id)[0].Equals("user"))
				{
					usersToAdd.Add(user);
				}
			}

			var viewModel = new TeamUsersViewModel
			{
				TeamId = (int)id,
				Users = usersToAdd
			};
			return View(viewModel);
		}

		[Authorize(Roles = "manager")]
		[HttpPost]
		public ActionResult AddMembers(TeamUser model)
		{
			var teamUser = new TeamUser
			{
				TeamId = model.TeamId,
				UserId = model.UserId
			};

			_context.TeamsUsers.Add(teamUser);
			_context.SaveChanges();

			return RedirectToAction("Members", new { id = model.TeamId });
		}


		[Authorize(Roles = "manager")]
		[HttpGet]
		public ActionResult RemoveMember(int id, string userId)
		{
			var teamUserToRemove = _context.TeamsUsers
				.SingleOrDefault(t => t.TeamId == id && t.UserId == userId);

			if (teamUserToRemove == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			_context.TeamsUsers.Remove(teamUserToRemove);
			_context.SaveChanges();
			return RedirectToAction("Members", new { id = id });
		}

		[HttpGet]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var teams = _context.TeamsUsers
				.Where(t => t.UserId.Equals(userId))
				.Include(t => t.Team)
				.Select(t => t.Team)
				.ToList();

			return View(teams);
		}
	}
}