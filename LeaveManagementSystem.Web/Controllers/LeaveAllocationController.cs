using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
{
    [Authorize(Roles.Administrator)]
    public async Task<IActionResult> Index()
    {
        var employees = await _leaveAllocationsService.GetEmployees();
        return View(employees);
    }
    
    
    [Authorize]
    // GET
    public async Task<IActionResult> Details(int? id)
    {
        var employeeVm = await _leaveAllocationsService.GetEmployeeAllocation(id);
        return View(employeeVm);
    }
}