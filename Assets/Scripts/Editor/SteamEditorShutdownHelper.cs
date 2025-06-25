#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SteamEditorShutdownHelper
{
    static SteamEditorShutdownHelper()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                if (SteamManager.Initialized)
                {
                    SteamManager.Instance.MarkQuitting();
                    Debug.Log("SteamManager marked quitting from editor.");
                }
            }
        };
    }
}
#endif