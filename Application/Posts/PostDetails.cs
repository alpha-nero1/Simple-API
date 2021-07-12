using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using System;
using Application.Core;


namespace Application.Posts
{
    public class PostDetails
    {
        public class Query : IRequest<HandlerResult<Post>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, HandlerResult<Post>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<HandlerResult<Post>> Handle(Query request, CancellationToken cancellationToken)
            {
                Post post = await _context.Posts.FindAsync(request.Id);
                return HandlerResult<Post>.Success(post);
            }
        }
    }
}