﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    [Authorize]
    public class UserInfosController : Controller
    {
        private ApplicationDbContext _context;
        public UserInfosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: UserInfos
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userInfo = _context.UsersInfos.SingleOrDefault(u => u.UserId.Equals(userId));

            if (userInfo == null) return HttpNotFound();

            return View(userInfo);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var userInfo = _context.UsersInfos.SingleOrDefault(u => u.UserId.Equals(userId));

            if (userInfo == null) return HttpNotFound();

            return View(userInfo);
        }
        [HttpPost]
        public ActionResult Edit(UserInfo userInfo)
        {
            var userInfoInDb = _context.UsersInfos.SingleOrDefault(u => u.UserId.Equals(userInfo.UserId));

            if (userInfo == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            userInfoInDb.FullName = userInfo.FullName;
            userInfoInDb.Age = userInfo.Age;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}