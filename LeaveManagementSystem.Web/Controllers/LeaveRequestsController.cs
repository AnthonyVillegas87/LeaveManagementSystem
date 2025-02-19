using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveRequestsController : Controller
{
    // Employee View requests
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    // Employee Create requests
    public async Task<IActionResult> Create()
    {
        return View();
    }
    
    // Employee Create requests
    [HttpPost]
    public async Task<IActionResult> Create(int create /*VM use*/)
    {
        return View();
    }
    
    // Employee Cancel requests
    [HttpPost]
    public async Task<IActionResult> Cancel(int leaveRequestId)
    {
        return View();
    }
    
    // Admin/Supervisor review requests
    public async Task<IActionResult> ListRequests()
    {
        return View();
    }
    
    // Admin/Supervisor requests
    public async Task<IActionResult> Review(int leaveRequestId)
    {
        return View();
    }
    
    // Admin/Supervisor requests
    [HttpPost]
    public async Task<IActionResult> Review(/*VM use*/)
    {
        return View();
    }
    
}