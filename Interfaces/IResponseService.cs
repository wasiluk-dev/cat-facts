namespace CatFacts.Interfaces;

public interface IResponseService<T>
{
    Task<T?> FetchResponseAsync(string url);
    List<T> ReadResponsesFromFile(string path);
    void SaveResponseToFile(string path, T response);
}