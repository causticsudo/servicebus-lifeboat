namespace ServiceBusLifeboat.Cli.Extensions;

public static class StringExtensions
{
    public static string MaskAfterCharCount(
        this string input,
        char charToMask,
        int charCount,
        int? outputLimit = null,
        char maskChar = '*')
    {
        int occurrenceCount = 0;
        int secondOccurrenceIndex = -1;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == charToMask)
            {
                occurrenceCount++;
                if (occurrenceCount == charCount)
                {
                    secondOccurrenceIndex = i;
                    break;
                }
            }
        }

        if (secondOccurrenceIndex >= 0 && secondOccurrenceIndex < input.Length - charCount)
        {
            string maskedPart = new string(maskChar, input.Length - secondOccurrenceIndex - charCount);
            string result = input.Substring(0, secondOccurrenceIndex + 1) + maskedPart;

            if (outputLimit.HasValue)
            {
                return TruncateString(result, outputLimit.Value);
            }

            return result;
        }

        return input;
    }

    public static string TruncateString(string input, int maxLength)
    {
        if (input.Length > maxLength)
        {
            return input.Substring(0, maxLength);
        }

        return input;
    }

    public static bool IsNullOrWhiteSpace(this string input)
        => String.IsNullOrWhiteSpace(input);
}