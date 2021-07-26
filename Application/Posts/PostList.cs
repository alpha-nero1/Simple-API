using MediatR;
using Domain;
using System.Collections.Generic;
using Persistence;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Posts
{
    // The request anmd handler classes are bundled inside, it's just a style although initially confusing.
    public class PostList
    {
        // This is simply the class that represents the request and what it looks like.
        // This class is what will be sent through via Mediator.Send()
        public class Query : IRequest<HandlerResult<List<PostDto>>> {}

        // This is simple the class that actually handles the request and returns a response.
        // This class is what will process the Mediator.Send() request.
        // It is NOT invoked directly, ever!
        public class Handler: IRequestHandler<Query, HandlerResult<List<PostDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<HandlerResult<List<PostDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Has potential to return cyclic error. Data needs reshaping.
                //var activites = await _context.Posts
                //    .Include(p => p.PostUsers)
                //    .ThenInclude(pu => pu.AppUser)
                //    .ToListAsync(cancellationToken);
                // Excellent projection!!
                var activites = await _context.Posts
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return HandlerResult<List<PostDto>>.Success(activites);
            }
        }
    }
}