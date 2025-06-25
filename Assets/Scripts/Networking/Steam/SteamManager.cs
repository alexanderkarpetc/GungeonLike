#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using System.Text;
using AOT;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    protected static bool s_EverInitialized = false;
    protected static SteamManager s_instance;
    protected bool m_bInitialized = false;
    private bool isQuitting = false;

    public static bool Initialized => s_instance != null && s_instance.m_bInitialized;
    public static SteamManager Instance => s_instance;

    protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    [MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
    protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText.ToString());
    }

#if UNITY_2019_3_OR_NEWER
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitOnPlayMode()
    {
        s_EverInitialized = false;
        s_instance = null;
    }
#endif

    protected virtual void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);

        if (s_EverInitialized)
        {
            Debug.LogWarning("SteamManager already initialized.");
            return;
        }

        if (!Packsize.Test())
            Debug.LogError("[Steamworks.NET] Packsize Test failed.");

        if (!DllCheck.Test())
            Debug.LogError("[Steamworks.NET] DllCheck failed.");

        try
        {
            if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
            {
                Application.Quit();
                return;
            }
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("[Steamworks.NET] Could not load SteamAPI DLL.\n" + e);
            Application.Quit();
            return;
        }

        m_bInitialized = SteamAPI.Init();
        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed.");
            return;
        }

        s_EverInitialized = true;
    }

    protected virtual void OnEnable()
    {
        if (s_instance == null)
            s_instance = this;

        if (!m_bInitialized)
            return;

        if (m_SteamAPIWarningMessageHook == null)
        {
            m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }

    protected virtual void Update()
    {
        if (!m_bInitialized || isQuitting)
            return;
        if (!Initialized) return;

        SteamAPI.RunCallbacks();
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        SteamAPI.Shutdown();
        m_bInitialized = false;
        s_instance = null;
    }

    private void OnDisable()
    {
        isQuitting = true;
    }
#else
    public static bool Initialized => false;
#endif
    public void MarkQuitting()
    {
        isQuitting = true;
    }
}