using System;
using System.Collections.Generic;
using System.Linq;

class Day04_SecureContainer
{
    public static List<int> GetValidCodesInRange_Part1(int min, int max)
    {
        var validCodes = new List<int>();
        for (int i = min; i < max; i++)
        {
            if (IsCodeValid_Part1(i, min, max))
            {
                validCodes.Add(i);
            }
        }

        return validCodes;
    }
    public static List<int> GetValidCodesInRange_Part2(int min, int max)
    {
        var validCodes = new List<int>();
        for (int i = min; i < max; i++)
        {
            if (IsCodeValid_Part2(i, min, max))
            {
                validCodes.Add(i);
            }
        }

        return validCodes;
    }

    private static int[] IntToArray(int code)
    {
        return code.ToString().Select(c => Convert.ToInt32(c.ToString())).ToArray();
    }

    private static bool IsCodeInRange(int code, int min, int max)
    {
        return min <= code && code < max;
    }

    private static bool DoDigitsIncrease(int code)
    {
        var codeArray = IntToArray(code);

        for (int i = 1; i < codeArray.Count(); i++)
        {
            var current = codeArray[i];
            var previous = codeArray[i - 1];
            if (previous > current) return false;
        }
        return true;
    }

    private static bool HasAtLeatOneRepeating(int code)
    {
        return IntToArray(code).GroupBy(x => x).Any(x => x.Count() > 1);
    }

    private static bool AreRepeatsValid(int code)
    {
        if (!HasAtLeatOneRepeating(code)) return true;
        var codeArray = IntToArray(code);
        var repeatingGroups = codeArray.GroupBy(x => x).Where(x => x.Count() >= 2);

        if (repeatingGroups.Count(x => x.Count() == 2) == 1) return true;

        if (repeatingGroups.Any(x => x.Count() >= 2))
        {
            if (repeatingGroups.All(x => x.Count() == 2)) return true;
        }

        return false;
    }

    private static bool IsCodeValid_Part1(int code, int min, int max)
    {
        return IsCodeInRange(code, min, max) && DoDigitsIncrease(code) && HasAtLeatOneRepeating(code);
    }

    private static bool IsCodeValid_Part2(int code, int min, int max)
    {
        return IsCodeInRange(code, min, max) && DoDigitsIncrease(code) && HasAtLeatOneRepeating(code) && AreRepeatsValid(code);
    }
}