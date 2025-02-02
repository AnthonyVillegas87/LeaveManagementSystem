namespace LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;

public class EmployeeAllocationVm
{
    public string Id { get; set; }
    
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Display(Name = "Email Address")]
    public string Email { get; set; }
    
    [Display(Name = "Date of Birth")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
    
    public List<LeaveAllocationVm> LeaveAllocations { get; set; }
}