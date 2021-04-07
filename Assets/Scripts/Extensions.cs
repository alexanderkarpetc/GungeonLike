using System;
using System.Collections.Generic;

public static class Extensions
{
    public static void NullSafeInvoke(this Action action)
    {
        action?.Invoke();
    }
    public static void NullSafeInvoke<T>(this Action<T> action, T value)
    {
        action?.Invoke(value);
    }
    public static TM GetValueOrDefault<T, TM>(this Dictionary<T, TM> dict, T key)
    {
        var res = dict.TryGetValue(key, out var value);
        return res ? value : default;
    }

    public static bool IsNullOrEmpty<T, TM>(this Dictionary<T, TM> dict)
    {
        return dict == null || dict.Count == 0;
    }
}