namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeReadOnlyVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal NumberOfDays { get; set; }
    
}