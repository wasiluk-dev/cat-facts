namespace CatFacts.Interfaces;

public interface IFileService
{
    void AppendAllText(string path, string contents);
    void Delete(string path);
    bool Exists(string path);
    IEnumerable<string> ReadLines(string path);
}