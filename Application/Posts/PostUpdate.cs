using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts
{
    // Allows us to add/remove post users on a post or lets a post owner soft delete the post.
    public class PostUpdate
    {
        public class Command : IRequest<HandlerResult<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, HandlerResult<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<HandlerResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .Include(p => p.PostUsers)
                    .ThenInclude(pu => pu.AppUser)
                    .SingleOrDefaultAsync(p => p.Id == request.Id);
                if (post == null) return null;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return null;
                var ownername = post.PostUsers.FirstOrDefault(pu => pu.IsOwner).AppUser.UserName;
                var contextUserPostUser = post.PostUsers.FirstOrDefault(pu => pu.AppUser.UserName == _userAccessor.GetUsername());
                
                if (contextUserPostUser != null)
                {
                    if (ownername == user.UserName)
                    {
                        // Toggle the disabled state.
                        post.DateDisabled = post.DateDisabled == null ? DateTime.Now : null;
                    }
                    if (ownername != user.UserName)
                    {
                        // If user is not an owner then take them off the list
                        post.PostUsers.Remove(contextUserPostUser);
                    }
                }
                else
                {
                    var postUser = new PostUser
                    {
                        AppUser = user,
                        Post = post,
                        IsOwner = false
                    };
                    post.PostUsers.Add(postUser);
                }

                var res = await _context.SaveChangesAsync() > 0;
                return res ? HandlerResult<Unit>.Success(Unit.Value) : HandlerResult<Unit>.Failure("Issue updateing post with post user.");
            }
        }
    }
}
