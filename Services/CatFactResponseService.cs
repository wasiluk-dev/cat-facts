using System.Text.Json;
using CatFacts.Interfaces;
using CatFacts.Models;

namespace CatFacts.Services;

public class CatFactResponseService(IHttpClientFactory httpClientFactory, IFileService fileService)
    : IResponseService<CatFact>
{
    public async Task<CatFact?> FetchResponseAsync(string url)
    {
        HttpClient client = httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<CatFact>(await response.Content.ReadAsStringAsync());
    }

    public void SaveResponseToFile(string path, CatFact response)
    {
        fileService.AppendAllText(path, $"\"{response.Fact}\",{response.Length}");
    }
}