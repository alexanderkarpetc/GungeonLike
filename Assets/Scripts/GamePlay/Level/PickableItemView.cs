using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Player;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level
{
    public class PickableItemView : NetworkBehaviour
    {
        [HideInInspector] public Weapon Weapon;
        [HideInInspector] public Dictionary<AmmoKind, int> Ammo;
        public ResourcePack ResourceValue;

        private int _index;

        [Serializable]
        public class ResourcePack
        {
            public ResourceKind resourceKind;
            public int amount;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && IsServer)  // Ensure only the server handles the trigger detection
            {
                var playerNetworkObject = other.GetComponent<NetworkObject>();
                if (playerNetworkObject != null)
                {
                    // Notify the specific client that they should pick up the weapon
                    NotifyClientToPickWeaponClientRpc(playerNetworkObject.OwnerClientId);
                }
            }
        }

        [ClientRpc]
        private void NotifyClientToPickWeaponClientRpc(ulong clientId, ClientRpcParams clientRpcParams = default)
        {
            // Only the client with the corresponding clientId will execute this code
            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                HandlePickUp();
            }
        }

        private void HandlePickUp()
        {
            if (Weapon != null)
            {
                PickWeapon(Weapon);
            }

            if (Ammo != null && Ammo.Count > 0)
            {
                AppModel.PlayerState().Backpack.AddAmmo(Ammo);
            }

            if (ResourceValue != null)
            {
                AppModel.PlayerState().Backpack.AddResource(ResourceValue.resourceKind, ResourceValue.amount);
            }

            DestroyItemOnServerRpc();
        }

        private void PickWeapon(Weapon weapon)
        {
            AppModel.PlayerState().Backpack.AddWeapon(weapon);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyItemOnServerRpc()
        {
            Destroy(gameObject);
        }
    }
}