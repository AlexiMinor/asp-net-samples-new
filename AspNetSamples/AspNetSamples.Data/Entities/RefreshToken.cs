using System.ComponentModel.DataAnnotations;
using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class RefreshToken : IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public Guid Value { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}