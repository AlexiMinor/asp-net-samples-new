using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class User : IBaseEntity
{
    public int Id { get; set; }

    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public int RoleId { get; set; }
    public Role Role{ get; set; }

    public List<Comment> Comments { get; set; }
}