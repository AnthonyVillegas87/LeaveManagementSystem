using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        var data = new TestViewModel
        {
            Name = "Anthony Villegas",
            DateOfBirth = new DateTime(1987, 02, 28)
        };
        return View(data);
    }
}