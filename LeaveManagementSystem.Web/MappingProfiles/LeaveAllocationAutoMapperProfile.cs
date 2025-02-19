using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.MappingProfiles;

public class LeaveAllocationAutoMapperProfile : Profile
{
    public LeaveAllocationAutoMapperProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationVm>();
        CreateMap<LeaveAllocation, LeaveAllocationEditVm>();
        CreateMap<ApplicationUser, EmployeeListVm>();
        CreateMap<Period, PeriodVm>();
    }
}