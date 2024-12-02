﻿using CatFacts.Interfaces;
using CatFacts.Models;
using CatFacts.Services;
using Microsoft.Extensions.DependencyInjection;

const string ApiUrl = "https://catfact.ninja/fact";
const string FactsNotFoundMessage = "You haven't saved any cat facts yet, give it a try!";
const string FileName = "facts.txt";

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
        CatFact? response = await responseService.FetchResponseAsync(ApiUrl);

        if (response != null)
        {
            Console.Clear();
            Console.WriteLine(response.Fact + Environment.NewLine);
            responseService.SaveResponseToFile(FileName, response);
        }
    }
}