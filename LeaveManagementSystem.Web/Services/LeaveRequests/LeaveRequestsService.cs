using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
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
       var currentDate = DateTime.Now;
       var period = await context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
       var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
       var allocationToDeduct = await context.LeaveAllocations
           .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId
                            && q.EmployeeId == user.Id
                            && q.PeriodId == period.Id);
       
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

    public async Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests()
    {
        var leaveRequests = await context.LeaveRequest
            .Include(q => q.Type)
            .ToListAsync();
        var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestListVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveRequestStatus = (LeaveRequestStatusEnum) q.LeaveRequestStatusId,
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
        }).ToList();
        
        var model = new EmployeeLeaveRequestListVM
        {
            ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
            PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
            DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
            TotalNumberOfRequests = leaveRequests.Count,
            LeaveRequests = leaveRequestModels
        };
        return model;
    }

    public async Task CancelLeaveRequest(int id)
    {
       var leaveRequest = await context.LeaveRequest.FindAsync(id);
       leaveRequest.LeaveRequestStatusId = (int) LeaveRequestStatusEnum.Canceled;
       
       
       var currentDate = DateTime.Now;
       var period = await context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
       var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
       var allocation = await context.LeaveAllocations
           .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId 
                            && q.EmployeeId == leaveRequest.EmployeeId
                            && q.PeriodId == period.Id);
       
       allocation.Days += numberOfDays;
       
       await context.SaveChangesAsync();
    }

    public async Task ReviewLeaveRequest(int leaveRequestId, bool isApproved)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
        var leaveRequest = await context.LeaveRequest.FindAsync(leaveRequestId);
        leaveRequest.LeaveRequestStatusId = isApproved ? (int)LeaveRequestStatusEnum.Approved : (int)LeaveRequestStatusEnum.Declined;


        leaveRequest.ReviewerId = user.Id;

        if (!isApproved)
        {
            var currentDate = DateTime.Now;
            var period = await context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
            var allocation = await context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId 
                                 && q.EmployeeId == leaveRequest.EmployeeId
                                 && q.PeriodId == period.Id);
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            allocation.Days += numberOfDays;
        }
        await context.SaveChangesAsync();
    }

    public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVm model)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
        var currentDate = DateTime.Now;
        var period = await context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
        var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
        var allocation = await context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId 
                             && q.EmployeeId == user.Id
                             && q.PeriodId == period.Id);
        
        return allocation.Days < numberOfDays;
    }

    public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
    {
        var leaveRequest = await context.LeaveRequest
            .Include(q => q.Type)
            .FirstAsync(q => q.Id == id);
        var user = await userManager.FindByIdAsync(leaveRequest.EmployeeId);
        var model = new ReviewLeaveRequestVM
        {
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Id = leaveRequest.Id,
            LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
            NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
            LeaveType = leaveRequest.Type?.Name,
            RequestComments = leaveRequest.RequestComments, 
            Employee = new EmployeeListVm()
            {
                Id = leaveRequest.EmployeeId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }
        };
        return model;
    }
}

