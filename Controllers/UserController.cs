using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [Route("signin")]
        public IActionResult Signin()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [Route("new")]
        public IActionResult New()
        {
            return View("New");
        }
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            int? ActiveUserId = HttpContext.Session.GetInt32("ActiveUserId");
            if(ActiveUserId == null)
                return RedirectToAction("Index");
            List<User> AllUsers = _userDashboardContext.User
            .ToList();
            ViewBag.AllUsers = AllUsers;
            ViewBag.ActiveUserId = (int)HttpContext.Session.GetInt32("ActiveUserId");
            return View("Dashboard");
        }
        [Route("users/show/{ProfileId}")]
        public IActionResult Show(int ProfileId)
        {
            int? ActiveUserId = HttpContext.Session.GetInt32("ActiveUserId");
            if(ActiveUserId == null)
                return RedirectToAction("Index");
            User showUser = _userDashboardContext.User
                .Where(u => u.UserId == ProfileId)
                .SingleOrDefault();
            List<Message> showMessages = _userDashboardContext.Message
                .Include(m => m.MessagePoster)
                .Include(m => m.MessageComments)
                    .ThenInclude(c => c.CommentPoster)
                .Where(m => m.ProfileMessagedUserId == ProfileId)
                .ToList();
            ViewBag.showUser = showUser;
            ViewBag.showMessages = showMessages;
            ViewBag.ActiveUserId = (int)HttpContext.Session.GetInt32("ActiveUserId");
            return View("Profile");
        }
        [Route("edit/{UserId}")]
        public IActionResult Edit(int UserId)
        {
            int? ActiveUserId = HttpContext.Session.GetInt32("ActiveUserId");
            if(ActiveUserId == null)
                return RedirectToAction("Index");
            User showEditUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            ViewBag.showEditUser = showEditUser;
            return View("Edit");
        }
        [HttpPost]
        [Route("user/create")]
        public IActionResult Create(Register model)
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
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.RegisterMessages = "Email Taken!";
                }
            }
            return View("New");
        }
        [Route("user/register")]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var EmailCheck = _userDashboardContext.User.Any(User => User.Email == model.Email);
                var CheckUserCount = _userDashboardContext.User.Any();
                if (EmailCheck == false)
                {
                    if(CheckUserCount == true)
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
                        HttpContext.Session.SetInt32("ActiveUserId", newUser.UserId);
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        User newUser = new User
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Password = model.Password,
                            Level = 1,
                            Description = "",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _userDashboardContext.Add(newUser);
                        _userDashboardContext.SaveChanges();
                        HttpContext.Session.SetInt32("ActiveUserId", newUser.UserId);
                        return RedirectToAction("Dashboard");
                    }
                }
                else
                {
                    ViewBag.RegisterMessages = "Email Taken!";
                }
            }
            return View("Index");
        }
        [Route("/user/login")]
        public IActionResult Login(Login model)
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
            return View("Login");
        }
        [Route("/user/{UserId}/EditInfo")]
        public IActionResult EditInfo(EditInfo model, int UserId)
        {
            if (ModelState.IsValid)
            {
                User EmailCheck = _userDashboardContext.User.SingleOrDefault(User => User.Email == model.Email);
                User ActiveUser = _userDashboardContext.User.SingleOrDefault(User => User.UserId == HttpContext.Session.GetInt32("ActiveUserId"));
                if (EmailCheck == null || model.Email == ActiveUser.Email)
                {
                    User editUser = _userDashboardContext.User
                        .Where(u => u.UserId == UserId)
                        .SingleOrDefault();
                        {
                            editUser.FirstName = model.FirstName;
                            editUser.LastName = model.LastName;
                            editUser.Email = model.Email;
                            editUser.UpdatedAt = DateTime.Now; 
                        };
                    _userDashboardContext.Update(editUser);
                    _userDashboardContext.SaveChanges();
                    return RedirectToAction("Show", new { ProfileId = UserId });
                }
                else
                {
                    ViewBag.RegisterMessages = "Email Taken!";
                }
            }
            User showEditUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            ViewBag.showEditUser = showEditUser;
            return View("Edit");
        }
        [Route("/user/{UserId}/EditDescription")]
        public IActionResult EditDescription(User model, int UserId)
        {
            if (ModelState.IsValid)
            {
                User editUser = _userDashboardContext.User
                    .Where(u => u.UserId == UserId)
                    .SingleOrDefault();
                    {
                        editUser.Description = model.Description;
                        editUser.UpdatedAt = DateTime.Now;
                    };
                _userDashboardContext.Update(editUser);
                _userDashboardContext.SaveChanges();
                return RedirectToAction("Show", new { ProfileId = UserId });
            }
            User showEditUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            ViewBag.showEditUser = showEditUser;
            return View("Edit");
        }
        [Route("/user/{UserId}/ChangePassword")]
        public IActionResult ChangePassword(ChangePassword model, int UserId)
        {
            if (ModelState.IsValid)
            {
                User editUser = _userDashboardContext.User
                    .Where(u => u.UserId == UserId)
                    .SingleOrDefault();
                    {
                        editUser.Password = model.Password;
                        editUser.UpdatedAt = DateTime.Now;
                    };
                _userDashboardContext.Update(editUser);
                _userDashboardContext.SaveChanges();
                return RedirectToAction("Show", new { ProfileId = UserId });
            }
            User showEditUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            ViewBag.showEditUser = showEditUser;
            return View("Edit");
        }
        [Route("/delete/{UserId}")]
        public IActionResult Delete(int UserId)
        {
            List<Comment> userComments = _userDashboardContext.Comment
                .Where(c => c.UserId == UserId)
                .ToList();
            foreach(var comment in userComments)
            {
                _userDashboardContext.Comment.Remove(comment);
                _userDashboardContext.SaveChanges();
            }
            List<Message> userMessages = _userDashboardContext.Message
                .Include(c => c.MessageComments)
                .Where(m => m.MessagePosterUserId == UserId)
                .ToList();
            foreach(var message in userMessages)
            {
                if(message.MessageComments.Count > 0)
                {
                    foreach(var comment in message.MessageComments)
                    {
                        _userDashboardContext.Comment.Remove(comment);
                        _userDashboardContext.SaveChanges();
                    }
                }
                _userDashboardContext.Message.Remove(message);
                _userDashboardContext.SaveChanges();
            }
            List<Message> userProile = _userDashboardContext.Message
                .Include(c => c.MessageComments)
                .Where(m => m.ProfileMessagedUserId == UserId)
                .ToList();
            foreach(var message in userProile)
            {
                if(message.MessageComments.Count > 0)
                {
                    foreach(var comment in message.MessageComments)
                    {
                        _userDashboardContext.Comment.Remove(comment);
                        _userDashboardContext.SaveChanges();
                    }
                }
                _userDashboardContext.Message.Remove(message);
                _userDashboardContext.SaveChanges();
            }
            User deleteUser = _userDashboardContext.User
                .Where(u => u.UserId == UserId)
                .SingleOrDefault();
            _userDashboardContext.User.Remove(deleteUser);
            _userDashboardContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [Route("user/message")]
        public IActionResult Message(Message model, int ProfileId)
        {
            if (ModelState.IsValid)
            {
                Message newMessage = new Message
                {
                    MessageText = model.MessageText,
                    MessagePosterUserId = (int)HttpContext.Session.GetInt32("ActiveUserId"),
                    ProfileMessagedUserId = ProfileId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _userDashboardContext.Add(newMessage);
                _userDashboardContext.SaveChanges();
                return RedirectToAction("Show", new { ProfileId = ProfileId });
            }
            User showUser = _userDashboardContext.User
                .Where(u => u.UserId == ProfileId)
                .SingleOrDefault();
            List<Message> showMessages = _userDashboardContext.Message
                .Include(m => m.MessagePoster)
                .Include(m => m.MessageComments)
                    .ThenInclude(c => c.CommentPoster)
                .Where(m => m.ProfileMessagedUserId == ProfileId)
                .ToList();
            ViewBag.showUser = showUser;
            ViewBag.showMessages = showMessages;
            ViewBag.ActiveUserId = (int)HttpContext.Session.GetInt32("ActiveUserId");
            return View("Profile");
        }
        [Route("/message/{MessageId}/delete/{ProfileId}")]
        public IActionResult DeleteMessage(int MessageId, int ProfileId)
        {
            Message deleteMessage = _userDashboardContext.Message
                .Include(m => m.MessageComments)
                .Where(m => m.MessageId == MessageId)
                .SingleOrDefault();
            foreach(var comment in deleteMessage.MessageComments)
            {
                _userDashboardContext.Comment.Remove(comment);
            }
            _userDashboardContext.Message.Remove(deleteMessage);
            _userDashboardContext.SaveChanges();
            return RedirectToAction("Show", new { ProfileId = ProfileId });
        }
        [Route("user/comment")]
        public IActionResult Comment(Comment model, int MessageId, int ProfileId)
        {
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment
                {
                    CommentText = model.CommentText,
                    UserId = (int)HttpContext.Session.GetInt32("ActiveUserId"),
                    MessageId = MessageId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _userDashboardContext.Add(newComment);
                _userDashboardContext.SaveChanges();
                return RedirectToAction("Show", new { ProfileId = ProfileId });
            }
            User showUser = _userDashboardContext.User
                .Where(u => u.UserId == ProfileId)
                .SingleOrDefault();
            List<Message> showMessages = _userDashboardContext.Message
                .Include(m => m.MessagePoster)
                .Include(m => m.MessageComments)
                    .ThenInclude(c => c.CommentPoster)
                .Where(m => m.ProfileMessagedUserId == ProfileId)
                .ToList();
            ViewBag.Messages = "Comment cannot be blank!";
            ViewBag.showUser = showUser;
            ViewBag.showMessages = showMessages;
            ViewBag.ActiveUserId = (int)HttpContext.Session.GetInt32("ActiveUserId");
            return View("Profile");
        }
        [Route("/comment/{CommentId}/delete/{ProfileId}")]
        public IActionResult DeleteComment(int CommentId, int ProfileId)
        {
            Comment deleteComment = _userDashboardContext.Comment
                .Where(c => c.CommentId == CommentId)
                .SingleOrDefault();
            _userDashboardContext.Comment.Remove(deleteComment);
            _userDashboardContext.SaveChanges();
            return RedirectToAction("Show", new { ProfileId = ProfileId });
        }
    }
}