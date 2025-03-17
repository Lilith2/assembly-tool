﻿using dnlib.DotNet;
using System.Text;

namespace ReCodeItLib.Utils;

public static class SysTypeExtentions
{
    /// <summary>
    /// Returns a string trimmed after any non letter character
    /// </summary>
    /// <param name="str"></param>
    /// <returns>Trimmed string if special character found, or the original string</returns>
    public static string TrimAfterSpecialChar(this UTF8String str)
    {
        var sb = new StringBuilder();

        var trimChars = new char[] { '`', '[', ']' };

        foreach (char c in str.ToString())
        {
            if (trimChars.Contains(c))
            {
            }

            if (char.IsLetter(c) || char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                return sb.ToString();
            }
        }

        if (sb.Length > 0)
        {
            return sb.ToString();
        }

        return str;
    }

    /// <summary>
    /// Returns a string trimmed after any non letter character
    /// </summary>
    /// <param name="str"></param>
    /// <returns>Trimmed string if special character found, or the original string</returns>
    public static string TrimAfterSpecialChar(this string str)
    {
        var sb = new StringBuilder();

        var trimChars = new char[] { '`', '[', ']' };

        foreach (char c in str)
        {
            if (trimChars.Contains(c))
            {
            }

            if (char.IsLetter(c) || char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                return sb.ToString();
            }
        }

        if (sb.Length > 0)
        {
            return sb.ToString();
        }

        return str;
    }

    /// <summary>
    /// Does the property or field name exist in a given list, this applies prefixes and handles capitalization.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="list"></param>
    /// <returns>True if it in the list</returns>
    public static bool IsFieldOrPropNameInList(this UTF8String str, List<string> list)
    {
        if (str.Trim().StartsWith("_"))
        {
            str = str.Replace("_", "");
        }

        var result = list.Any(item => str.StartsWith(item, StringComparison.CurrentCultureIgnoreCase));

        return result;
    }

    /// <summary>
    /// Does the property or field name exist in a given list, this applies prefixes and handles capitalization.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="list"></param>
    /// <returns>True if it in the list</returns>
    public static bool IsFieldOrPropNameInList(this string str, List<string> list)
    {
        if (str.Trim().StartsWith("_"))
        {
            str = str.Replace("_", "");
        }

        var result = list.Any(item => str.StartsWith(item, StringComparison.CurrentCultureIgnoreCase));

        return result;
    }
}