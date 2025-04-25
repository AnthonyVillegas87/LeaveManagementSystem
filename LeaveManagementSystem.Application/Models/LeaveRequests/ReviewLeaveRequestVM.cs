using System.ComponentModel;
using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestListVM
{
    public EmployeeListVm Employee { get; set; } = new EmployeeListVm();
    
    [DisplayName("Additional Information")]
    public string RequestComments { get; set; }
}