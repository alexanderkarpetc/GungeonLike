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

    public static bool IsNullOrEmpty<T, TM>(this Dictionary<T, TM> dict)
    {
        return dict == null || dict.Count == 0;
    }
}