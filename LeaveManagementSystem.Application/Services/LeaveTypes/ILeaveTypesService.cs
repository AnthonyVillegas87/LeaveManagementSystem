using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.Services.LeaveTypes;

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
   Task<bool> MaxDaysExceededAsync(int leaveTypeId, decimal modelDays);
}