namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class LeaveRequestsService : ILeaveRequestService
{
    public Task CreateLeaveRequest(LeaveRequestCreateVm model)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests()
    {
        throw new NotImplementedException();
    }

    public Task<LeaveRequestListVM> GetAllLeaveRequests()
    {
        throw new NotImplementedException();
    }

    public Task CancelLaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public Task ReviewLaveRequest(ReviewLeaveRequestVM model)
    {
        throw new NotImplementedException();
    }
}