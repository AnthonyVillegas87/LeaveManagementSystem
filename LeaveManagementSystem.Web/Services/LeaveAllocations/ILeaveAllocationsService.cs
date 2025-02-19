using LeaveManagementSystem.Web.Models.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeave(string employeeId);
    Task<List<LeaveAllocation>> GetLeaveAllocations(string? employeeId);
    Task<EmployeeAllocationVm> GetEmployeeAllocations(string? employeeId);
    Task<LeaveAllocationEditVm> GetEmployeeAllocation(int allocationId);
    Task<List<EmployeeListVm>> GetEmployees();
    Task EditAllocation(LeaveAllocationEditVm model);
}