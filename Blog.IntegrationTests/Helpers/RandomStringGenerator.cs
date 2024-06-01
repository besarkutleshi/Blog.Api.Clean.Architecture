using System.Text;

namespace Blog.IntegrationTests.Helpers;

public class RandomStringGenerator
{
    private static readonly Random _random = new Random();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int minLength, int maxLength)
    {
        if (minLength < 0 || maxLength < 0 || minLength > maxLength)
        {
            throw new ArgumentException("Invalid minimum or maximum length values.");
        }

        int length = _random.Next(minLength, maxLength + 1);
        var stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(_chars[_random.Next(_chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}