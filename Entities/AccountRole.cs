using Microsoft.AspNetCore.Identity;

namespace Entities;

public class AccountRole : IdentityUserRole<Guid>
{
    public Account Account { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
