using Application.Posts;
using AutoMapper;
using Domain;
using System.Linq;

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
            // Excellent example of mapping a list from objects that are nested in the join table objects.
            CreateMap<PostUser, Profiles.Profile>()
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.AppUser.Email));
            // Create a mapping profile for posts to return to apis, useful once we have included models.
            CreateMap<Post, PostDto>()
                .ForMember(
                    d => d.OwnerUsername, 
                    opts =>
                    {
                        opts.MapFrom(s => s.PostUsers.FirstOrDefault(x => x.IsOwner).AppUser.UserName);
                    }
                );
        }
    }
}