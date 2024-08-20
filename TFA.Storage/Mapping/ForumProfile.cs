using AutoMapper;

namespace TFA.Storage.Mapping;

public class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Forum, Domain.Models.Forum>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
    }
}