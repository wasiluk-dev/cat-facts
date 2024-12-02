using CatFacts.Interfaces;

namespace CatFacts.Services;

public class FileService : IFileService
{
    public void AppendAllText(string path, string contents)
    {
        File.AppendAllText(path, contents + Environment.NewLine);
    }

    public void Delete(string path)
    {
        File.Delete(path);
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public IEnumerable<string> ReadLines(string path)
    {
        return File.ReadLines(path);
    }
}