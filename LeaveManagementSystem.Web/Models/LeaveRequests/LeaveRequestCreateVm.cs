using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class LeaveRequestCreateVm
{
    [DisplayName("Start Date")]
    public DateOnly StartDate { get; set; }
    
    [DisplayName("End Date")]
    public DateOnly EndDate { get; set; }
    
    [DisplayName("Desired Leave Type")]
    public int LeaveTypeId { get; set; }
    
    [DisplayName("Additional Information")]
    public string? RequestComments { get; set; }
    public SelectList LeaveTypes { get; set; }
}