namespace CatFacts.Interfaces;

/// <summary>
/// Provides methods for operating on files.
/// </summary>
public interface IFileService
{
    /// <inheritdoc cref="File.AppendAllText(string, string)"/>
    void AppendAllText(string path, string contents);
    /// <inheritdoc cref="File.Delete(string)"/>
    void Delete(string path);
    /// <inheritdoc cref="File.Exists(string)"/>
    bool Exists(string path);
    /// <inheritdoc cref="File.ReadLines(string)"/>
    IEnumerable<string> ReadLines(string path);
}