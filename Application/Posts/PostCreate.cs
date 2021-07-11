using MediatR;
using Domain;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Application.Posts
{
    public class PostCreate
    {
        // Commands do NOT return data.
        public class Command : IRequest
        {
            public Post Post { get; set; }
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
                // Just track the fact that we have added a post.
                _context.Posts.Add(request.Post);
                await _context.SaveChangesAsync();
                // Let controller know we have completed processing.
                return Unit.Value;
            }
        }
    }
}