const string ApiUrl = "https://catfact.ninja/fact";
const string FileName = "facts.txt";

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
}