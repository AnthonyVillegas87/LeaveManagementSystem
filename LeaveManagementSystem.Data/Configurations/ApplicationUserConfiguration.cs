using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.HasData(new ApplicationUser
        {
            Id = "df7cd3e7-b3c2-44d0-9027-aad95c5b89c5",
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            NormalizedUserName = "ADMIN@LOCALHOST.COM",
            UserName = "admin@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "Admin",
            DateOfBirth = new DateOnly(1980, 1, 1),
        });
    }
}