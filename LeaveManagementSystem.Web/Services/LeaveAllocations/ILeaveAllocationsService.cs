using LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeave(string employeeId);
    Task<List<LeaveAllocation>> GetLeaveAllocations(string? employeeId);
    Task<EmployeeAllocationVm> GetEmployeeAllocation(string? employeeId);
    Task<List<EmployeeListVm>> GetEmployees();
}