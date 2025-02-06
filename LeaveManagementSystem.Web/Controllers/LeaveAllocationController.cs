using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
{
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Index()
    {
        var employees = await _leaveAllocationsService.GetEmployees();
        return View(employees);
    }
    
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> AllocateLeave(string employeeId)
    {
         await _leaveAllocationsService.AllocateLeave(employeeId);
        return RedirectToAction(nameof(Details), new { id = employeeId });
    }
    
    // GET
    public async Task<IActionResult> Details(string? employeeId)
    {
        var employeeVm = await _leaveAllocationsService.GetEmployeeAllocation(employeeId);
        return View(employeeVm);
    }
}