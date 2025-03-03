using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

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
       leaveRequest.LeaveRequestStatusId = (int) LeaveRequestStatusEnum.Pending;
       
       // save leave request to the db
       context.Add(leaveRequest);
       
       //deduct allocation days based on request
       var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
       var allocationToDeduct = await context.LeaveAllocations
           .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);
       
       allocationToDeduct.Days -= numberOfDays;
       await context.SaveChangesAsync();

    }
    

    public async Task<List<LeaveRequestListVM>> GetEmployeeLeaveRequests()
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
        var leaveRequests = await context.LeaveRequest
            .Include(q => q.Type)
            .Where(q => q.EmployeeId == user.Id)
            .ToListAsync();
        var model = leaveRequests.Select(q => new LeaveRequestListVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveType = q.Type?.Name,
            LeaveRequestStatus = (LeaveRequestStatusEnum) q.LeaveRequestStatusId,
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
        }).ToList();
        return model;
    }

    public Task<LeaveRequestListVM> GetAllLeaveRequests()
    {
        throw new NotImplementedException();
    }

    public Task CancelLeaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public Task ReviewLeaveRequest(ReviewLeaveRequestVM model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVm model)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
        var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
        var allocation = await context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);
        
        return allocation.Days < numberOfDays;
    }
}

