namespace ServiceBusLifeboat.Domain.Extensions;
using static Console;

public static class StringExtension
{
    public static List<ulong> ConvertToLongList(this string input)
    {
        var numbersString = input.Split(',');

        var numbersLong = new List<ulong>();

        foreach (var number in numbersString)
        {
            if (ulong.TryParse(number.Trim(), out var result))
            {
                numbersLong.Add(result);
            }
            else
            {
                WriteLine($"'{number}' is not valid and will be ignored");
            }
        }

        return numbersLong;
    }
}