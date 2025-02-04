using LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeave(string employeeId);
    Task<List<LeaveAllocation>> GetLeaveAllocations(int? employeeId);
    Task<EmployeeAllocationVm> GetEmployeeAllocation(int? employeeId);
    Task<List<EmployeeListVm>> GetEmployees();
}