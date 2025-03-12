using LeaveManagementSystem.Web.Models.LeaveAllocations;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestListVM
{
    public EmployeeListVm Employee { get; set; } = new EmployeeListVm();
    
    [DisplayName("Additional Information")]
    public string RequestComments { get; set; }
}