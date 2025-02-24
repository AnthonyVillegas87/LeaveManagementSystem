using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveRequestsController(ILeaveTypesService leaveTypesService) : Controller
{
    // Employee View requests
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    // Employee Create requests
    public async Task<IActionResult> Create()
    {
        var leaveTypes = await leaveTypesService.GetAllAsync();
        var leaveTypesList = new SelectList(leaveTypes, "Id", "Name");
        var model = new LeaveRequestCreateVm()
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            LeaveTypes = leaveTypesList
        };
        return View(model);
    }
    
    // Employee Create requests
    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequestCreateVm model)
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