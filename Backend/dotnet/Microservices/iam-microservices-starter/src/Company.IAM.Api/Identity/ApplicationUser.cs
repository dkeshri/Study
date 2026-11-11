using Microsoft.AspNetCore.Identity;

namespace Company.IAM.Api.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
}