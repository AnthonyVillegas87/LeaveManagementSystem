using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
{
    public async Task AllocateLeave(string employeeId)
    {
        //Get all leave types
        var leaveTypes = await context.LeaveTypes.ToListAsync();
        
        //Get the current period based on the year
        var currentDate = DateTime.Now;
        var period = await  context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
        var monthsRemaining = period.EndDate.Month - currentDate.Month;
        //Calculate leave based on the # of months left in the period
        
        //FOREACH leave type, create an allocation entry
        foreach (var leaveType in leaveTypes)
        {
            var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12);
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveType.Id,
                PeriodId = period.Id,
                Days = accrualRate * monthsRemaining,
            };
            context.Add(leaveAllocation);
        }
        await context.SaveChangesAsync();
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocations()
    {

        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        var leaveAllocations = await context.LeaveAllocations
            .Include(q => q.LeaveType)
            .Include(q => q.Period)
            .Where(q => q.EmployeeId == user.Id)
            .ToListAsync();
        
        return leaveAllocations;
    }

    public async Task<EmployeeAllocationVm> GetEmployeeAllocation()
    {
        var allocations = await GetLeaveAllocations();
        var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVm>>(allocations);
        
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        var employeeVm = new EmployeeAllocationVm
        {
            DateOfBirth = user.DateOfBirth,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Id = user.Id,
            LeaveAllocations = allocationVmList
        };
        
        return employeeVm;
    }
}