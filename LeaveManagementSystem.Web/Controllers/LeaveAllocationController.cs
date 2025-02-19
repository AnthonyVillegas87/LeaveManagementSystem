using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService) : Controller
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAllocation(LeaveAllocationEditVm model)
    {
        // Step 1. Validate that the logic is within our parameters
        if (await _leaveTypesService.MaxDaysExceededAsync(model.LeaveType.Id, model.Days))
        {
            ModelState.AddModelError("Days", "You have exceeded the max days for this leave type.");
        }

        // Step 2. IF above statement is true, proceed to update
        if (ModelState.IsValid)
        {
            await _leaveAllocationsService.EditAllocation(model);
            return RedirectToAction(nameof(Details), new { employeeId = model.EmployeeList.Id});
        }
        
        // When NOT valid, track # of days submitted in form. 
        var days = model.Days;
        // Reload all data within the datastore
        model = await _leaveAllocationsService.GetEmployeeAllocation(model.Id);
        // Override the data
        model.Days = days;
        // Reload the view
        return View(model);
    }
    
    // GET
    public async Task<IActionResult> Details(string? employeeId)
    {
        var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(employeeId);
        return View(employeeVm);
    }
}