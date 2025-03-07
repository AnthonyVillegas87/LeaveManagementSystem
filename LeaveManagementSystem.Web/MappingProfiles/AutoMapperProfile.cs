using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeaveType, LeaveTypeReadOnlyVm>();
        CreateMap<LeaveTypeCreateVm, LeaveType>();
        CreateMap<LeaveTypeEditVm,LeaveType>().ReverseMap();
    }
}