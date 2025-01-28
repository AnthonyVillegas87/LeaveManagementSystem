using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data;

public class LeaveType : BaseEntity
{
    public string Name { get; set; }
    
    [Column(TypeName = "decimal(5, 2)")]
    public decimal NumberOfDays { get; set; }
    
}