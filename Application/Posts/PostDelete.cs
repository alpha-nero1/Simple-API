using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Persistence;


namespace Application.Posts
{
    public class PostDelete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Post post = await _context.Posts.FindAsync(request.Id);
                // Hard deletion.
                _context.Remove(post);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}