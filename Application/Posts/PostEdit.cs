using MediatR;
using Domain;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using Persistence;

namespace Application.Posts
{
    public class PostEdit
    {
        public class Command : IRequest
        {
            public Post Post { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Post post = await _context.Posts.FindAsync(request.Post.Id);
                // Map from source object into destination object.
                _mapper.Map(request.Post, post);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}