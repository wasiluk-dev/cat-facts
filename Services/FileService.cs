using CatFacts.Interfaces;

namespace CatFacts.Services;

public class FileService : IFileService
{
    /// <inheritdoc/>
    public void AppendAllText(string path, string contents)
    {
        File.AppendAllText(path, contents + Environment.NewLine);
    }

    /// <inheritdoc/>
    public void Delete(string path)
    {
        File.Delete(path);
    }

    /// <inheritdoc/>
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    /// <inheritdoc/>
    public IEnumerable<string> ReadLines(string path)
    {
        return File.ReadLines(path);
    }
}