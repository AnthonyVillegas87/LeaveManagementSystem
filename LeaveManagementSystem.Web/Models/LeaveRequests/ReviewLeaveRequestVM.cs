using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestListVM
{
    public EmployeeListVm Employee { get; set; } = new EmployeeListVm();
}