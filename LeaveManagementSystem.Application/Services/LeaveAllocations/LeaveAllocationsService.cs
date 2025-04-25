using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext context, IMapper _mapper, IUserService userService,IPeriodsService periodsService) : ILeaveAllocationsService
{
    public async Task AllocateLeave(string employeeId)
    {
        //Get all leave types
        var leaveTypes = await context.LeaveTypes
            .Where(q => !q.LeaveAllocations.Any(q => q.EmployeeId == employeeId))
            .ToListAsync();
        
        //Get the current period based on the year
        var period = await periodsService.GetCurrentPeriod();
        var monthsRemaining = period.EndDate.Month - DateTime.Now.Month;
        
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
                Days = accrualRate * monthsRemaining
            };
            context.Add(leaveAllocation);
        }
        await context.SaveChangesAsync();
    }
    public async Task<EmployeeAllocationVm> GetEmployeeAllocations(string? employeeId)
    {
        var user = string.IsNullOrEmpty(employeeId) ? await userService.GetLoggedInUser() : await userService.GetUserById(employeeId);
        var allocations = await GetLeaveAllocations(employeeId);
        var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVm>>(allocations);
        
        var leaveTypesCount = await context.LeaveTypes.CountAsync();
        var employeeVm = new EmployeeAllocationVm
        {
            DateOfBirth = user.DateOfBirth,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Id = user.Id,
            LeaveAllocations = allocationVmList,
            IsCompletedAllocation = leaveTypesCount == allocations.Count
        };
        
        return employeeVm;
    }
    public async Task<List<EmployeeListVm>> GetEmployees()
    {
        var users = await userService.GetEmployees();
        var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVm>>(users.ToList());
        return employees;
    }
    public async Task<LeaveAllocationEditVm> GetEmployeeAllocation(int allocationId)
    {
        var allocation = await context.LeaveAllocations
            .Include(q => q.LeaveType)
            .Include(q => q.Employee)
            .FirstOrDefaultAsync(q => q.Id == allocationId);
        var model = _mapper.Map<LeaveAllocationEditVm>(allocation);
        return model;
    }
    public async Task EditAllocation(LeaveAllocationEditVm model)
    {
        await context.LeaveAllocations
            .Where(q => q.Id == model.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, model.Days));
    }
    public async Task<List<LeaveAllocation>> GetLeaveAllocations(string? employeeId)
    {
        string userId = string.Empty;
        if (!string.IsNullOrEmpty(employeeId))
        {
            employeeId = userId;
        }
        else
        {
            var user = await userService.GetLoggedInUser();
            employeeId = user.Id;
        }
        
        var currentDate = DateTime.Now;
        // var period = await  context.Periods.SingleOrDefaultAsync(q => q.EndDate.Year == currentDate.Year);
        var leaveAllocations = await context.LeaveAllocations
            .Include(q => q.LeaveType)
            .Include(q => q.Period)
            .Where(q => q.EmployeeId == employeeId && q.Period.EndDate.Year == currentDate.Year)
            .ToListAsync();
        
        return leaveAllocations;
    }

    public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
    {
        var period = await periodsService.GetCurrentPeriod();
        var allocation = await context.LeaveAllocations
            .FirstAsync(q => q.LeaveTypeId == leaveTypeId 
                                      && q.EmployeeId == employeeId 
                                      && q.PeriodId == period.Id);
        return allocation;
    }
    private async Task<bool> AllocationExists(string employeeId, int periodId, int leaveTypeId)
    {
        var exists = await context.LeaveAllocations.AnyAsync(q => 
            q.EmployeeId == employeeId
            && q.PeriodId == periodId
            && q.LeaveTypeId == leaveTypeId
        );
        return exists;
    }
    
}