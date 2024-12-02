namespace CatFacts.Interfaces;

/// <summary>
/// Provides methods for operating on Api responses.
/// </summary>
/// <typeparam name="TModel">Type of model representing an Api response.</typeparam>
public interface IResponseService<TModel>
{
    /// <summary>
    /// Fetches an Api response from specified Url.
    /// </summary>
    /// <param name="url">Api endpoint to request data from.</param>
    /// <returns>A <typeparamref name="TModel"/> instance of the response when the method completes.</returns>
    Task<TModel?> FetchResponseAsync(string url);
    /// <summary>
    /// Reads formatted Api responses from specified file.
    /// </summary>
    /// <param name="path">File to read responses from.</param>
    /// <returns>A List containing model instances of read responses.</returns>
    List<TModel> ReadResponsesFromFile(string path);
    /// <summary>
    /// Appends a formatted Api response to specified file.
    /// </summary>
    /// <param name="path">File to append response data to.</param>
    /// <param name="response">Response to append to specified file.</param>
    void SaveResponseToFile(string path, TModel response);
}