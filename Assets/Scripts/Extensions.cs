using System;

public static class Extensions
{
    public static void NullSafeInvoke(this Action action)
    {
        action?.Invoke();
    }
}