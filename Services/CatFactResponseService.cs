using System.Text.Json;
using System.Text.RegularExpressions;
using CatFacts.Interfaces;
using CatFacts.Models;

namespace CatFacts.Services;

public partial class CatFactResponseService(IHttpClientFactory httpClientFactory, IFileService fileService)
    : IResponseService<CatFact>
{
    [GeneratedRegex("""(?<=")(.+)(?=",[0-9]+)""")]
    private static partial Regex FactRegex();
    [GeneratedRegex("""(?<=",)[0-9]+(?=)""")]
    private static partial Regex LengthRegex();
    
    public async Task<CatFact?> FetchResponseAsync(string url)
    {
        HttpClient client = httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<CatFact>(await response.Content.ReadAsStringAsync());
    }

    public List<CatFact> ReadResponsesFromFile(string path)
    {
        List<CatFact> responses = [];
        foreach (string line in fileService.ReadLines(path))
        {
            string fact = FactRegex().Match(line).Value;
            int length = Convert.ToInt32(LengthRegex().Match(line).Value);

            responses.Add(new CatFact(fact, length));
        }

        return responses;
    }

    public void SaveResponseToFile(string path, CatFact response)
    {
        // escape double quotes before saving
        string fact = response.Fact.Replace("\"", "\"\"");
        fileService.AppendAllText(path, $"\"{fact}\",{response.Length}");
    }
}