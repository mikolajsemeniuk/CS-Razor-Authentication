using Microsoft.AspNetCore.Identity;

namespace Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<AccountRole> AccountRoles { get; set; } = null!;
}
