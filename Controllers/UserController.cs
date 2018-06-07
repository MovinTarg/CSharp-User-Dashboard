using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Dashboard.Models;

namespace User_Dashboard.Controllers
{
    public class UserController : Controller
    {
        private UserDashboardContext _userDashboardContext;
        public UserController (UserDashboardContext context)
        {
            _userDashboardContext = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [HttpPost]
        [Route("/user/register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User EmailCheck = _userDashboardContext.User.SingleOrDefault(User => User.Email == model.Email);
                if (EmailCheck == null)
                {
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Level = 0,
                        Description = "",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _userDashboardContext.Add(newUser);
                    _userDashboardContext.SaveChanges();
                    int ActiveUserId = _userDashboardContext.User.Last().UserId;
                    HttpContext.Session.SetInt32("ActiveUserId", ActiveUserId);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.RegisterMessages = "Email Taken!";
                }
            }
            return View("Index");
        }
        [Route("/user/login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User ActiveUser = _userDashboardContext.User.SingleOrDefault(User => User.Email == model.Email);
                if(ActiveUser != null)
                {
                    if(model.Password == ActiveUser.Password)
                    {
                        HttpContext.Session.SetInt32("ActiveUserId", ActiveUser.UserId);
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ViewBag.Messages = "Incorrect Email / Password";
                    }
                }
            }
            return View("Index");
        }
        [Route("/user/{UserId}/delete")]
        public IActionResult Delete(int UserId)
        {
            User deleteUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            var ProfileMessages = _userDashboardContext.Message.Where(m => m.ProfileId == UserId);
            foreach(var message in ProfileMessages)
            {
                var MessageComments = _userDashboardContext.Comment.Where(c => c.MessageId == message.MessageId);
                foreach(var comment in MessageComments)
                {
                    _userDashboardContext.Comment.Remove(comment);
                }
                _userDashboardContext.Message.Remove(message);
            }
            _userDashboardContext.User.Remove(deleteUser);
            _userDashboardContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
