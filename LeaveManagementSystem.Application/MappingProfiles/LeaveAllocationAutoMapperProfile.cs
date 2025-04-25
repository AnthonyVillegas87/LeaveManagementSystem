using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.MappingProfiles;

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