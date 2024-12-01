﻿using CatFacts.Interfaces;
using CatFacts.Models;
using CatFacts.Services;
using Microsoft.Extensions.DependencyInjection;

const string ApiUrl = "https://catfact.ninja/fact";
const string FileName = "facts.txt";

ServiceProvider serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddSingleton<IFileService, FileService>()
    .AddTransient<IResponseService<CatFact>, CatFactResponseService>()
    .BuildServiceProvider();

IResponseService<CatFact> responseService = serviceProvider.GetRequiredService<IResponseService<CatFact>>();

while (true)
{
    Console.WriteLine("Available commands:" + Environment.NewLine +
                      "– 'fact' to get a cool cat fact," + Environment.NewLine + 
                      "– 'list' to see all the previously fetched facts," + Environment.NewLine + 
                      "– 'clear' to delete all the stored facts," + Environment.NewLine + 
                      "– 'exit' to exit the app.");
    
    string? input = Console.ReadLine()?.Trim().ToLower();
    Console.Clear();
    
    if (input == "exit")
    {
        break;
    }

    if (input == "fact")
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