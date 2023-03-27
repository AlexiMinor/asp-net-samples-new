using Microsoft.AspNetCore.Authorization;

namespace AspNetSamples.Mvc.Auth;

public class MinAgeRequirement : IAuthorizationRequirement
{
    public int MinAge { get; set; }

    public MinAgeRequirement(int minAge)
    {
        MinAge = minAge;
    }
}