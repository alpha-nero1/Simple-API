using MediatR;
using Domain;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.Core;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts
{
    public class PostCreate
    {
        // Commands do NOT return data.
        public class Command : IRequest<HandlerResult<Unit>>
        {
            public Post Post { get; set; }
        }

        // A class specifying validation for the command that is sent in to the handler.
        // Allows us to add validation at the Application layer to validate the command.
        public class CommandValidator : AbstractValidator<Command> {
            
            public CommandValidator()
            {
                RuleFor(x => x.Post).SetValidator(new PostValidator());
            }
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
                // Get the current user so that we can auto-assign a post user.
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

                var postUser = new PostUser
                {
                    AppUser = user,
                    Post = request.Post,
                    IsOwner = true
                };
                // This will track the post user along with the post to be created,
                // all done via ef!
                request.Post.PostUsers.Add(postUser);

                // Just track the fact that we have added a post.
                _context.Posts.Add(request.Post);
                bool success = await _context.SaveChangesAsync() > 0;
                if (!success) return HandlerResult<Unit>.Failure("Failure to create post");
                // Let controller know we have completed processing.
                return HandlerResult<Unit>.Success(Unit.Value);
            }
        }
    }
}