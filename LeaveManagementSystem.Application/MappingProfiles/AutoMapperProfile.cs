using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeaveType, LeaveTypeReadOnlyVm>();
        CreateMap<LeaveTypeCreateVm, LeaveType>();
        CreateMap<LeaveTypeEditVm,LeaveType>().ReverseMap();
    }
}