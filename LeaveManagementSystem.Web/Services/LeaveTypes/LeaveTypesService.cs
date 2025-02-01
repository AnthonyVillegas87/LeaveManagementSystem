using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveTypes;


public class LeaveTypesService(ApplicationDbContext context, IMapper mapper) : ILeaveTypesService
{
    public async Task<List<LeaveTypeReadOnlyVm>> GetAllAsync()
    {
        // SELECT * FROM LeaveTypes
        var data = await context.LeaveTypes.ToListAsync();
        // convert the datamodel into a view model - use AutoMapper
        var viewData = mapper.Map<List<LeaveTypeReadOnlyVm>>(data);
        return viewData;
    }

    public async Task<T?> GetByIdAsync<T>(int id) where T : class
    {
        var data = await context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        
        if(data == null) return null;
        
        var viewData = mapper.Map<T>(data);
        return viewData;
    }

    public async Task RemoveByIdAsync(int id)
    {
        var data = await context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (data != null)
        {
            context.Remove(data);
           await context.SaveChangesAsync();
        }
    }

    public async Task EditAsync(LeaveTypeEditVm model)
    {
        var leaveType = mapper.Map<LeaveType>(model);
        context.Update(leaveType);
        await context.SaveChangesAsync();
    }

    public async Task CreateAsync(LeaveTypeCreateVm model)
    {
        var leaveType = mapper.Map<LeaveType>(model);
        context.Add(leaveType);
        await context.SaveChangesAsync();
    }
    
    
    
    public bool LeaveTypeExists(int id)
    {
        return context.LeaveTypes.Any(e => e.Id == id);
    }

    public async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        var lowerCaseName = name.ToLower();
        return await context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(lowerCaseName));
    }

    public async Task<bool> EditCheckIfLeaveTypeNameExists(LeaveTypeEditVm leaveTypeEdit)
    {
        var lowerCaseName = leaveTypeEdit.Name.ToLower();
        return await context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(lowerCaseName) && e.Id != leaveTypeEdit.Id);
    }
    public async Task<bool> CheckIfNumberOfDaysExceeded(decimal numberOfDays)
    {
        return await context.LeaveTypes.AnyAsync(t => t.NumberOfDays > 24.0m);
    }
}