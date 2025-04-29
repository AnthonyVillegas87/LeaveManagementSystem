using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
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
    public async Task<IActionResult> Create(int? leaveTypeId)
    {
        var leaveTypes = await leaveTypesService.GetAllAsync();
        var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
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
        await leaveRequestService.CancelLeaveRequest(leaveRequestId);
        return RedirectToAction(nameof(Index));
    }
    
    // Admin/Supervisor review requests
    [Authorize(Policy = "AdminSupervisorOnly")]
    public async Task<IActionResult> ListRequests()
    {
        var model = await leaveRequestService.GetAllLeaveRequests();
        return View(model);
    }
    
    // Admin/Supervisor requests
    public async Task<IActionResult> Review(int id)
    {
        var model = await leaveRequestService.GetLeaveRequestForReview(id);
        return View(model);
    }
    
    // Admin/Supervisor requests
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(int id, bool approved)
    {
        await leaveRequestService.ReviewLeaveRequest(id, approved);
        return RedirectToAction(nameof(ListRequests));
    }
    
}