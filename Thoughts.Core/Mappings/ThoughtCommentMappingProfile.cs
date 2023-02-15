using AutoMapper;
using Thoughts.Core.Requests.ThoughtComment;
using Thoughts.Core.Responses.ThoughtComment;
using Thoughts.Domain.Entities;

namespace Thoughts.Core.Mappings
{
	public class ThoughtCommentMappingProfile : Profile
	{
		public ThoughtCommentMappingProfile()
		{
			CreateMap<ThoughtComment, ThoughtCommentsResponse>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
			CreateMap<AddCommentRequest, ThoughtComment>();
		}
	}
}
