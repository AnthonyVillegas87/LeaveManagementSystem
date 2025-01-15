using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services;

public interface ILeaveTypesService
{
    Task<List<LeaveTypeReadOnlyVm>> GetAllAsync();
    Task<T?> GetByIdAsync<T>(int id) where T : class;
    Task RemoveByIdAsync(int id);
    Task EditAsync(LeaveTypeEditVm model);
    Task CreateAsync(LeaveTypeCreateVm model);
    bool LeaveTypeExists(int id);
    Task<bool> CheckIfLeaveTypeNameExists(string name);
    Task<bool> EditCheckIfLeaveTypeNameExists(LeaveTypeEditVm leaveTypeEdit);
    Task<bool> CheckIfNumberOfDaysExceeded(decimal numberOfDays);
}