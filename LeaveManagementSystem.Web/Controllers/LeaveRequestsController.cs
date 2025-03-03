using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveRequestsController(ILeaveTypesService leaveTypesService, ILeaveRequestService leaveRequestService) : Controller
{
    // Employee View requests
    public async Task<IActionResult> Index()
    {
        var model = await leaveRequestService.GetEmployeeLeaveRequests();
        return View(model);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveRequestCreateVm model)
    {
        if (await leaveRequestService.RequestDatesExceedAllocation(model))
        {
            ModelState.AddModelError(string.Empty, "The number of leave requests exceeds the allocation.");
            ModelState.AddModelError(nameof(model.EndDate), "End date cannot be in the future.");
        }
        if (ModelState.IsValid)
        {
            await leaveRequestService.CreateLeaveRequest(model);
            return RedirectToAction(nameof(Index));
        }
        var leaveTypes = await leaveTypesService.GetAllAsync();
        model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
        return View(model);
    }
    
    // Employee Cancel requests
    [HttpPost]
    [ValidateAntiForgeryToken]
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(/*VM use*/)
    {
        return View();
    }
    
}