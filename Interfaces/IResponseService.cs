namespace CatFacts.Interfaces;

public interface IResponseService<T>
{
    Task<T?> FetchResponseAsync(string url);
    void SaveResponseToFile(string path, T response);
}