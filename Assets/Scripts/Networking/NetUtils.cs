using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    public static class NetUtils
    {
        public static void DestroyIfNotMine(NetworkBehaviour networkBehaviour)
        {
            if(!networkBehaviour.IsOwner)
                Object.Destroy(networkBehaviour);
        }
    }
}