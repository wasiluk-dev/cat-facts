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
    
    /// <inheritdoc/>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout, or the response is unsuccessful.</exception>
    /// <exception cref="TaskCanceledException">The request failed due to timeout.</exception>
    public async Task<CatFact?> FetchResponseAsync(string url)
    {
        HttpClient client = httpClientFactory.CreateClient();
        
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            return JsonSerializer.Deserialize<CatFact>(await response.Content.ReadAsStringAsync());
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void SaveResponseToFile(string path, CatFact response)
    {
        // escape double quotes before saving
        string fact = response.Fact.Replace("\"", "\"\"");
        fileService.AppendAllText(path, $"\"{fact}\",{response.Length}");
    }
}