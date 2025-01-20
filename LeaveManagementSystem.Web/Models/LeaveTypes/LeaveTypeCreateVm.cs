namespace LeaveManagementSystem.Web.Models.LeaveTypes;

public class LeaveTypeCreateVm
{
    [Required]
    [Length(4, 150, ErrorMessage = "Length requirement violation.")]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(1, 90)]
    [Display(Name = "Maximum Allocation of Days")]
    public decimal NumberOfDays { get; set; }
}