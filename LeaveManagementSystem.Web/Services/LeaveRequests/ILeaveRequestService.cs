namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public interface ILeaveRequestService
{
    Task CreateLeaveRequest(LeaveRequestCreateVM model);
    Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests();
    Task<LeaveRequestListVM> GetAllLeaveRequests();
    Task CancelLaveRequest(int id);
    Task ReviewLaveRequest(ReviewLeaveRequestVM model);
}