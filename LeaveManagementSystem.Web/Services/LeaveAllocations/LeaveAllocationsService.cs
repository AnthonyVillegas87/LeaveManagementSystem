using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext context) : ILeaveAllocationsService
{
    public async Task AllocateLeave(string employeeId)
    {
        //Get all leave types
        var leaveTypes = await context.LeaveTypes.ToListAsync();
        
        //Get the current period based on the year
        var currentDate = DateTime.Now;
        var period = await  context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
        var monthsRemaining = period.EndDate.Month - currentDate.Month;
        //Calculate leave based on the # of months left in the period
        
        //FOREACH leave type, create an allocation entry
        foreach (var leaveType in leaveTypes)
        {

            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveType.Id,
                PeriodId = period.Id,
                Days = leaveType.NumberOfDays / monthsRemaining
            };
            context.Add(leaveAllocation);
        }
        await context.SaveChangesAsync();
    }
}