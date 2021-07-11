using AutoMapper;
using Domain;

// AutoMapper and the Profile super class easily allow us to
// register mapping profiles between objects.
// This means we can easily copy the properties from one object to another.
// Something that is very trivial in other languages requires a little more care
// in C#.
namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Allow copy from post to post objects.
            CreateMap<Post, Post>();
        }
    }
}