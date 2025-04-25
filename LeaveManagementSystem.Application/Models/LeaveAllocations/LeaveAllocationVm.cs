using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.Models.LeaveAllocations;

public class LeaveAllocationVm
{
    public int Id { get; set; }
    
    [Display(Name = "Number Of Days")]
    public decimal Days { get; set; }

    [Display(Name = "Allocation Period")] 
    public PeriodVm Period { get; set; } = new PeriodVm();
    
    public LeaveTypeReadOnlyVm LeaveType { get; set; } = new LeaveTypeReadOnlyVm();
}