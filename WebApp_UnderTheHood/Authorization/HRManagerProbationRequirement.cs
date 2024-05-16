using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRManagerProbationRequirement :IAuthorizationRequirement
    {
        public int probationMonth;

        public HRManagerProbationRequirement(int probationMonth)
        {
            this.probationMonth = probationMonth;
        }
    }


    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if(!context.User.HasClaim(s=>s.Type== "Empdate"))
                return Task.CompletedTask;

            if(DateTime.TryParse(context.User.FindFirst(s=>s.Type== "Empdate")?.Value, out DateTime empDate))
            {
                var period= DateTime.Now -empDate;
                if (period.Days > 30 * requirement.probationMonth)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
