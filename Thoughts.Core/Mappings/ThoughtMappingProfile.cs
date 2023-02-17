using AutoMapper;
using Thoughts.Core.Requests.Thought;
using Thoughts.Core.Responses.Thought;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Mappings
{
    public class ThoughtMappingProfile : Profile
    {
        public ThoughtMappingProfile()
        {
			CreateMap<CreateThoughtRequest, Thought>();
			CreateMap<UpdateThoughtRequest, Thought>();
			CreateMap<Thought, ThoughtDto>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count))
				.ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count));

			CreateMap<Thought, ThoughtResponse>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count))
				.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.OrderByDescending(c => c.CreationDate)))
				.ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count));
		}

    }
}
