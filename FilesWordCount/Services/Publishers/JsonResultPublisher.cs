using System.Text.Json;
using AutoMapper;
using FilesWordCount.Interfaces;
using FilesWordCount.Models.Response;

namespace FilesWordCount.Services.Publishers;

public class JsonResultsPublisher : IResultPublisher
{
    private readonly IMapper _mapper;

    public JsonResultsPublisher(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public void Show(IEnumerable<(string, int)> values)
    {
        WordStatisticResponse response = _mapper.Map<WordStatisticResponse>(values);

        string output = JsonSerializer.Serialize(response, options: new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

        Console.WriteLine(output);
    }
}
