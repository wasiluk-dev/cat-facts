namespace CatFacts.Models;

public class CatFact(string fact, int length)
{
    public string Fact { get; init; } = fact;
    public int Length { get; init; } = length;
}