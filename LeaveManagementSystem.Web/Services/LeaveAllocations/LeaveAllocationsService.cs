using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
{
    public async Task AllocateLeave(string employeeId)
    {
        //Get all leave types
        var leaveTypes = await context.LeaveTypes
            .Where(q => !q.LeaveAllocations.Any(q => q.EmployeeId == employeeId))
            .ToListAsync();
        
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
                Days = accrualRate * monthsRemaining
            };
            context.Add(leaveAllocation);
        }
        await context.SaveChangesAsync();
    }

    public async Task<EmployeeAllocationVm> GetEmployeeAllocations(string? employeeId)
    {
        var user = string.IsNullOrEmpty(employeeId) ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User) : await _userManager.FindByIdAsync(employeeId);
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
        var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
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
        // var allocation = await GetEmployeeAllocation(model.Id);
        // if (allocation == null)
        // {
        //     throw new Exception("Leave allocation not found");
        // }
        // allocation.Days = model.Days;
        // context.Entry(allocation).State = EntityState.Modified;
        // await context.SaveChangesAsync();
        
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
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
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