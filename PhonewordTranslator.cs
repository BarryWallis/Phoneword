using System.Diagnostics;
using System.Text;

namespace Phoneword;

/// <summary>
/// Class to translate a phone number with letters to the correspondng phone digits (e.g., "abc" == 1, "def"==2).
/// </summary>
internal static class PhonewordTranslator
{
    private static readonly string[] _digits =
    {
        "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ"
    };

    /// <summary>
    /// Translate a raw string containing a phone number with digits and letters to a string containing only 
    /// digits.
    /// </summary>
    /// <param name="raw">The string to translate.</param>
    /// <returns>
    /// A valid phone number containing only digits or <see langword="null"/> if the raw string was 
    /// invalid for a phone number.
    /// </returns>
    public static string? ToNumber(string raw)
    {
        const string ValidPhoneNumberCharacters = "-1234567890";

        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        raw = raw.ToUpperInvariant();
        StringBuilder newNumber = new();
        foreach (char c in raw)
        {
            if (ValidPhoneNumberCharacters.Contains(c))
            {
                _ = newNumber.Append(c);
            }
            else
            {
                int? result = TranslateToNumber(c);
                if (result is not null)
                {
                    _ = newNumber.Append(result);
                }
                else
                {
                    return null;
                }
            }
        }

        Debug.Assert(raw.Length == newNumber.ToString().Length);
        Debug.Assert(newNumber.ToString().All(c => ValidPhoneNumberCharacters.Contains(c)));
        return newNumber.ToString();
    }

    /// <summary>
    /// Translate a letter to its corresponding telephone digit.
    /// </summary>
    /// <param name="c">The letter to translate.</param>
    /// <returns>The corresponding digit or <see langword="null"/> if the letter is invalid.</returns>
    private static int? TranslateToNumber(char c)
    {
        int? result = null;
        for (int i = 0; i < _digits.Length; i++)
        {
            if (_digits[i].Contains(char.ToUpperInvariant(c)))
            {
                result = 2 + i;
            }
        }

        Debug.Assert(result is null or (>= 0 and <= 9));
        return result;
    }
}
