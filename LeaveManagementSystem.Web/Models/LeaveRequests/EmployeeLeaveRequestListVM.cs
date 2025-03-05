using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class EmployeeLeaveRequestListVM
{
    [Display(Name = "Total Number of Requests")]
    public int TotalNumberOfRequests { get; set; }
    
    [Display(Name = "Approved Requests")]
    public int ApprovedRequests { get; set; }
    
    [Display(Name = "Declined Requests")]
    public int DeclinedRequests { get; set; }
    
    [Display(Name = "Pending Requests")]
    public int PendingRequests { get; set; }

    public List<LeaveRequestListVM> LeaveRequests { get; set; } = [];
}