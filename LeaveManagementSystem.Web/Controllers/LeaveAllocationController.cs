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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AllocateLeave(string? id)
    {
         await _leaveAllocationsService.AllocateLeave(id);
        return RedirectToAction(nameof(Details), new { employeeId = id } );
    }

    public async Task<IActionResult> EditAllocation(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
        if (allocation == null)
        {
            return NotFound();
        }
        return View(allocation);
    }
    
    // GET
    public async Task<IActionResult> Details(string? employeeId)
    {
        var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(employeeId);
        return View(employeeVm);
    }
}