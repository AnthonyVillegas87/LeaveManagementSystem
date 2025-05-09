using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.Periods;

public class PeriodsService(ApplicationDbContext context) : IPeriodsService
{
    public async Task<Period> GetCurrentPeriod()
    {
        var currentDate = DateTime.Now;
        var period = await context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
        return period;
    }
}