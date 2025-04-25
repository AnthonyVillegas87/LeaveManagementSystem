using System.Reflection;
using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagementSystem.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddScoped<ILeaveTypesService, LeaveTypesService>();
        services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestsService>();
        services.AddScoped<IPeriodsService, PeriodsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
}