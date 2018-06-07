using Microsoft.AspNetCore.Mvc;
using User_Dashboard.Models;

namespace User_Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        private UserDashboardContext _userDashboardContext;
        public DashboardController (UserDashboardContext context)
        {
            _userDashboardContext = context;
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            
            return View("Dashboard");
        }
    }
}