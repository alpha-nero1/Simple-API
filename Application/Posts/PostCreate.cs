using MediatR;
using Domain;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.Core;


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
            
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<HandlerResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
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