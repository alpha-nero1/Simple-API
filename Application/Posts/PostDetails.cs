using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Application.Posts
{
    public class PostDetails
    {
        public class Query : IRequest<HandlerResult<PostDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, HandlerResult<PostDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<HandlerResult<PostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                PostDto post = await _context.Posts
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                return HandlerResult<PostDto>.Success(post);
            }
        }
    }
}