using Steamworks;
using UnityEngine;

namespace Networking
{
    public class SteamSetup : MonoBehaviour
    {
        private void Start()
        {
            if (SteamManager.Initialized)
            {
                Debug.LogError("My SteamID: " + SteamUser.GetSteamID());
                var personaName = SteamFriends.GetPersonaName();
                Debug.Log(personaName);
            }
        }
    }
}