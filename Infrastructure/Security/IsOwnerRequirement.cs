using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    // Allows us to specify a custom authorization policy to make sure a user is a post owner before continuing.
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class IsOwnerRequirementHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        private readonly DataContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsOwnerRequirementHandler(DataContext dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
        {
            string userId = context.User.FindFirstValue(ClaimTypes.Name);
            if (userId == null) return Task.CompletedTask; // Not authed.
            // Get the post id from the http context.
            var postId = Guid.Parse(_httpContextAccessor.HttpContext?.Request?.RouteValues.SingleOrDefault(x => x.Key == "Id").Value?.ToString());
            // Because SaveChangesAsync is never called the EF never released the PU from memeory,
            // We must specify AsNoTracking to release it.
            var postUser = _dbcontext.PostUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUser.Id == userId && x.Post.Id == postId).Result;
            if (postUser == null) return Task.CompletedTask;
            // If is owner then flag that the authentication has succeeded.
            if (postUser.IsOwner) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
