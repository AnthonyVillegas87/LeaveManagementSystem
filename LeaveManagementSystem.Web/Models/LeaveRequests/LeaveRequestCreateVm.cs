namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public class LeaveRequestCreateVm
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int LeaveTypeId { get; set; }
    public string? RequestComments { get; set; }
}