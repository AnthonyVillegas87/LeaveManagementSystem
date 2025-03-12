using LeaveManagementSystem.Web.Models.LeaveAllocations;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestListVM
{
    public EmployeeListVm Employee { get; set; } = new EmployeeListVm();
    public string RequestComments { get; set; } = string.Empty;
}