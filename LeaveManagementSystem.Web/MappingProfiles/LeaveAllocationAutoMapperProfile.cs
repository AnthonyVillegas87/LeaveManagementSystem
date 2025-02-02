using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes.LeaveAllocations;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.MappingProfiles;

public class LeaveAllocationAutoMapperProfile : Profile
{
    public LeaveAllocationAutoMapperProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationVm>();
        CreateMap<Period, PeriodVm>();
    }
}