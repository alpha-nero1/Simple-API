using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Persistence;
using Application.Core;


namespace Application.Posts
{
    public class PostDelete
    {
        public class Command : IRequest<HandlerResult<Unit>>
        {
            public Guid Id { get; set; }
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
                Post post = await _context.Posts.FindAsync(request.Id);
                throw new Exception("heyaaaa");
                if (post == null) return null;
                // Hard deletion.
                _context.Remove(post);
                bool success = await _context.SaveChangesAsync() > 0;
                if (!success) return HandlerResult<Unit>.Failure("Failed to delete the post.");
                return HandlerResult<Unit>.Success(Unit.Value);
            }
        }
    }
}