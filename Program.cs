using System.Text.RegularExpressions;
using CatFacts.Interfaces;
using CatFacts.Models;
using CatFacts.Services;
using Microsoft.Extensions.DependencyInjection;

const string ApiUrl = "https://catfact.ninja/fact";
const string FileName = "facts.txt";
const string FactFetchingErrorMessage = "Couldn't get any cat facts right now, please try again later.";
const string FactsNotFoundMessage = "You haven't saved any cat facts yet, give it a try!";

ServiceProvider serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddSingleton<IFileService, FileService>()
    .AddTransient<IResponseService<CatFact>, CatFactResponseService>()
    .BuildServiceProvider();

IResponseService<CatFact> responseService = serviceProvider.GetRequiredService<IResponseService<CatFact>>();
IFileService fileService = serviceProvider.GetRequiredService<IFileService>();

while (true)
{
    Console.WriteLine("Press [ENTER] to get a cool cat fact!" + Environment.NewLine +
                      "Available commands:" + Environment.NewLine +
                      "– 'list' to see all the previously fetched facts," + Environment.NewLine + 
                      "– 'clear' to delete all the stored facts," + Environment.NewLine + 
                      "– 'exit' to exit the app.");
    
    string? input = Console.ReadLine()?.Trim().ToLower();
    Console.Clear();
    
    if (input == "exit")
    {
        break;
    }

    if (input == "list")
    {
        try
        {
            // get all the facts and remove duplicates
            List<CatFact> responses = responseService
                .ReadResponsesFromFile(FileName)
                .GroupBy(x => x.Fact)
                .Select(x => x.First())
                .ToList();
            
            // the file exists, but it's empty
            if (responses.Count == 0)
            {
                Console.WriteLine(FactsNotFoundMessage + Environment.NewLine);
                continue;
            }
            
            Console.WriteLine("Previously fetched cat facts:");
            foreach (CatFact response in responses)
            {
                Console.WriteLine("– " + response.Fact);
            }

            Console.WriteLine();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(FactsNotFoundMessage + Environment.NewLine);
        }
    }
    else if (input == "clear")
    {
        if (fileService.Exists(FileName))
        {
            fileService.Delete(FileName);
            Console.WriteLine("The cat facts file has been deleted :(" + Environment.NewLine);
            continue;
        }
        
        Console.WriteLine(FactsNotFoundMessage + Environment.NewLine);
    }
    else
    {
        Console.WriteLine("Fetching a fact, please wait...");
        try
        {
            CatFact? response = await responseService.FetchResponseAsync(ApiUrl);

            Console.Clear();
            if (response != null)
            {
                string fact = response.Fact;
                Regex regex = NotLastSentencePeriodOrBangRegex();
            
                if (regex.Match(fact).Success)
                {
                    fact = regex.Replace(fact, "?", 1);
                }
                else
                {
                    if (fact[^1] is '.' or '!')
                    {
                        fact = fact.Remove(fact.Length - 1, 1) + '?';
                    }
                    // the end of the sentence is missing punctuation
                    else
                    {
                        fact += '?';
                    }
                }
            
                Console.WriteLine("Did you know that..." + Environment.NewLine +
                                  $"...{fact[0].ToString().ToLower()}{fact[1..]}" + Environment.NewLine);
                responseService.SaveResponseToFile(FileName, response);
            }
            else
            {
                Console.WriteLine(FactFetchingErrorMessage + Environment.NewLine);
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            Console.WriteLine(FactFetchingErrorMessage + Environment.NewLine);
        }
    }
}

internal abstract partial class Program
{
    [GeneratedRegex("""[\.|!](?=\s+[A-Z])""")]
    private static partial Regex NotLastSentencePeriodOrBangRegex();
}