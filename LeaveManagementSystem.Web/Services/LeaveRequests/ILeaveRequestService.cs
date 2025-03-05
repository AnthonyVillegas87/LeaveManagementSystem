using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public interface ILeaveRequestService
{
    Task CreateLeaveRequest(LeaveRequestCreateVm model);
    Task<List<LeaveRequestListVM>> GetEmployeeLeaveRequests();
    Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests();
    Task CancelLeaveRequest(int id);
    Task ReviewLeaveRequest(ReviewLeaveRequestVM model);
    Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVm model);
}