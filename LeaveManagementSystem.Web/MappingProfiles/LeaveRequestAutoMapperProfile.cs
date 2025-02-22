using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.Periods;
using LeaveManagementSystem.Web.Services.LeaveRequests;

namespace LeaveManagementSystem.Web.MappingProfiles;

public class LeaveRequestAutoMapperProfile : Profile
{
    public LeaveRequestAutoMapperProfile()
    {
        CreateMap<LeaveRequestCreateVm, LeaveRequest>();
    }
}