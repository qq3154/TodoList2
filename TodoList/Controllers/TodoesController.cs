using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TodoList.Models;
using System.Data.Entity;
using TodoList.ViewModels;
using Microsoft.AspNet.Identity;

namespace TodoList.Controllers
{
	[Authorize]
    public class TodoesController : Controller
    {

		private ApplicationDbContext _context; // use ApplicationDbContext class to connect to database

		public TodoesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Todoes
		public ActionResult Index(string searchString)
		{
			var userId = User.Identity.GetUserId();

			var todoes = _context.Todoes
				.Include(t => t.Category)
				.Where(t => t.UserId.Equals(userId))
                .ToList();
			if (!searchString.IsNullOrWhiteSpace())
			{
				todoes = todoes.Where(t => t.Description.Contains(searchString)).ToList();
			}

			return View(todoes);
		}

		public ActionResult Details(int? id)
		{
			var userId = User.Identity.GetUserId();
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var todo = _context.Todoes
				.Include(t => t.Category)
				.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (todo == null) return HttpNotFound();

			return View(todo);
		}

		public ActionResult Delete(int? id)
		{			
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var userId = User.Identity.GetUserId();

			var todo = _context.Todoes
				.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (todo == null) return HttpNotFound();

			_context.Todoes.Remove(todo);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Create()
		{
			var viewModel = new TodoCategoriesViewModel()
			{
				Categories = _context.Categories.ToList()
		};			
		return View(viewModel);
		}

		[HttpPost]
		public ActionResult Create(Todo todo)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = new TodoCategoriesViewModel()
				{
					Todo = todo,
					Categories = _context.Categories.ToList()
				};
				return View(viewModel);
			}
			var userId = User.Identity.GetUserId();
			var newTodo = new Todo()
			{
				Description = todo.Description,
				CategoryId = todo.CategoryId,
				DueDate = todo.DueDate,
				UserId = userId
			};

			_context.Todoes.Add(newTodo);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Edit(int? id)
        {
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var userId = User.Identity.GetUserId();

			var todoInDb = _context.Todoes
				.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);
			if (todoInDb == null) return HttpNotFound();
			var viewModel = new TodoCategoriesViewModel()
			{
				Todo = todoInDb,
				Categories = _context.Categories.ToList()
			};

			return View(viewModel);
		}
		[HttpPost]
		public ActionResult Edit(Todo todo)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = new TodoCategoriesViewModel()
				{
					Todo = todo,
					Categories = _context.Categories.ToList()
				};
				return View(viewModel);
			}
			var userId = User.Identity.GetUserId();

			var todoInDb = _context.Todoes
				.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == todo.Id);

			if (todoInDb == null) return HttpNotFound();

			todoInDb.Description = todo.Description;
			todoInDb.DueDate = todo.DueDate;
			todoInDb.CategoryId = todo.CategoryId;

			_context.SaveChanges();

			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Report()
		{
			var viewModel = new List<SatisticalReportViewModel>();
			
			var userId = User.Identity.GetUserId();


			var todoesInDb = _context.Todoes
				.Include(t => t.Category)
				.Where(t => t.UserId.Equals(userId))
				.ToList();

			var todoesGroupByName = todoesInDb.GroupBy(t => t.Category.Name).ToList();

			foreach (var categoryGroup in todoesGroupByName)
			{
				var categoryQuantity = categoryGroup.Select(c => c.Category).Count();
				viewModel.Add(new SatisticalReportViewModel()
				{
					CategoryName = categoryGroup.Key,
					CategoryQuantity = categoryQuantity
				});
			}

			return View(viewModel);
		}

	}
}