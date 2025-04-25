using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations;

public interface ILeaveAllocationsService
{
    Task AllocateLeave(string employeeId);
    Task<List<LeaveAllocation>> GetLeaveAllocations(string? employeeId);
    Task<EmployeeAllocationVm> GetEmployeeAllocations(string? employeeId);
    Task<LeaveAllocationEditVm> GetEmployeeAllocation(int allocationId);
    Task<List<EmployeeListVm>> GetEmployees();
    Task EditAllocation(LeaveAllocationEditVm model);
    Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);
}