namespace LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;

public class EmployeeListVm
{
    public string Id { get; set; }
    
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Display(Name = "Email Address")]
    public string Email { get; set; }
}