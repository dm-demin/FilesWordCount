using AutoMapper;
using FilesWordCount.Models.Response;

namespace FilesWordCount.Mappers;

public static class WordStatisticMapper
{
    public static IMapper GetMapper()
    {
        MapperConfiguration configuration = new MapperConfiguration (
            cfg =>
            {
                cfg.CreateMap<KeyValuePair<string, int>, StatisticRecord>()
                            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Value))
                            .ForMember(dest => dest.Word, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<IEnumerable<(string, int)>, WordStatisticResponse>()
                            .ForMember(dest => dest.Statistics, opt => opt.MapFrom(src => src.Select(x => new KeyValuePair<string, int>(x.Item1, x.Item2)).ToList()));
            });

        return new Mapper(configuration);
    }
}
