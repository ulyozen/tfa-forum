using AutoMapper;

namespace TFA.Storage.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, Domain.Models.Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(t => t.TopicId));
    }
}