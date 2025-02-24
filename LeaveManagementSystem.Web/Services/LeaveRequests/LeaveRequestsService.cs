using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public partial class LeaveRequestsService(IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
    ApplicationDbContext context) : ILeaveRequestService
{
    public async Task CreateLeaveRequest(LeaveRequestCreateVm model)
    {
        // map data to leave request data model
       var leaveRequest = mapper.Map<LeaveRequest>(model);
       
       // get logged in employee id
       var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
       leaveRequest.EmployeeId = user.Id;
       
       // set LeaveRequestStatusId 
       leaveRequest.LeaveRequestStatusId = (int) LeaveRequestStatus.Pending;
       
       // save leave request to the db
       context.Add(leaveRequest);
       
       //deduct allocation days based on request
       var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
       var allocationToDeduct = await context.LeaveAllocations
           .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);
       
       allocationToDeduct.Days =- numberOfDays;
       await context.SaveChangesAsync();

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

