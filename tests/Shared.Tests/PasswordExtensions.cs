using Bogus;
using Bogus.DataSets;

namespace Auth.Api.Shared.Tests;

public static class PasswordExtensions
{
    public static string GeneratePassword(this Internet internet)
    {
        Randomizer? r = internet.Random;

        string? number = r.Replace("#"); // length 1
        string? letter = r.Replace("?"); // length 2
        string lowerLetter = letter.ToLower(); //length 3
        char symbol = r.Char((char)33, (char)47); //length 4 - ascii range 33 to 47

        string? padding = r.String2(r.Number(8, 12)); //length 6 - 10

        return new string(r.Shuffle(number + letter + lowerLetter + symbol + padding).ToArray());
    }
}
