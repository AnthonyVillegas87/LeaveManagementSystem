using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
{
    [Authorize]
    // GET
    public async Task<IActionResult> Details()
    {
       
        var employeeVm = await _leaveAllocationsService.GetEmployeeAllocation();
        return View(employeeVm);
    }
}