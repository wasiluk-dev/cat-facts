namespace CatFacts.Interfaces;

public interface IFileService
{
    void AppendAllText(string path, string contents);
    void Delete(string path);
    IEnumerable<string> ReadLines(string path);
}