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
        public class Command : IRequest<HandlerResult<Unit>>
        {
            public Post Post { get; set; }
        }

        public class Handler : IRequestHandler<Command, HandlerResult<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<HandlerResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Post post = await _context.Posts.FindAsync(request.Post.Id);
                if (post == null) return null;
                // Map from source object into destination object.
                _mapper.Map(request.Post, post);
                bool success = await _context.SaveChangesAsync() > 0;
                if (!success) return HandlerResult<Unit>.Failure("Failure to edit post.");
                return HandlerResult<Unit>.Success(Unit.Value);
            }
        }
    }
}