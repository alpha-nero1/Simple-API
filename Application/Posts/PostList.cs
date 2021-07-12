using MediatR;
using Domain;
using System.Collections.Generic;
using Persistence;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Core;


namespace Application.Posts
{
    // The request anmd handler classes are bundled inside, it's just a style although initially confusing.
    public class PostList
    {
        // This is simply the class that represents the request and what it looks like.
        // This class is what will be sent through via Mediator.Send()
        public class Query : IRequest<HandlerResult<List<Post>>> {}

        // This is simple the class that actually handles the request and returns a response.
        // This class is what will process the Mediator.Send() request.
        // It is NOT invoked directly, ever!
        public class Handler: IRequestHandler<Query, HandlerResult<List<Post>>>
        {
            private readonly DataContext _context;
            
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<HandlerResult<List<Post>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return HandlerResult<List<Post>>.Success(await _context.Posts.ToListAsync(cancellationToken));
            }
        }
    }
}