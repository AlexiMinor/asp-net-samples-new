using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AspNetSamples.Mvc.Auth;

public class MinAgeHandler : AuthorizationHandler<MinAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        MinAgeRequirement requirement)
    {
        if (context.User.HasClaim(cl=>cl.Type.Equals("Age")))
        {
            int.TryParse(context.User.FindFirst(claim => claim.Type.Equals("Age")).Value ,out var age);

            if (age >= requirement.MinAge)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}