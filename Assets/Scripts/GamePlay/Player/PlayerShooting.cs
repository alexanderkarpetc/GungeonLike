using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        [HideInInspector] public Weapon Weapon;
        [SerializeField] public Transform _weaponSlot;

        private void Update()
        {
            if (!IsOwner) return;  // Only the owning player can shoot
            CheckShoot();
        }

        private void CheckShoot()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // The mouse is over a UI element, so do not process other mouse input.
                return;
            }

            if (Weapon == null)
            {
                Weapon = _weaponSlot.GetChild(0).GetComponent<Weapon>();
                Weapon.IsPlayers = true;
            }

            if (Weapon.ShootingType == WeaponShootingType.SemiAuto && Input.GetMouseButtonDown(0))
            {
                ShootServerRpc();  // Request to shoot on the server
                return;
            }      
            if (Weapon.ShootingType == WeaponShootingType.Automatic && Input.GetMouseButton(0))
            {
                ShootServerRpc();  // Request to shoot on the server
                return;
            }
            if (Weapon.ShootingType == WeaponShootingType.Charged && Input.GetMouseButton(0))
            {
                StartChargeServerRpc();  // Request to start charging the weapon on the server
                return;
            }
            if (Weapon.ShootingType == WeaponShootingType.Charged && !Input.GetMouseButton(0))
            {
                StopChargeServerRpc();  // Request to stop charging the weapon on the server
                return;
            }
        }

        [ServerRpc]
        private void ShootServerRpc()
        {
            // Broadcast shooting to all clients
            ShootClientRpc();
        }

        [ClientRpc]
        private void ShootClientRpc()
        {
            if (Weapon != null)
            {
                Weapon.TryShoot();  // Execute the shooting action
            }
        }

        [ServerRpc]
        private void StartChargeServerRpc()
        {
            // Broadcast the charge start to all clients
            StartChargeClientRpc();
        }

        [ClientRpc]
        private void StartChargeClientRpc()
        {
            if (Weapon is JetEngineWeapon jetEngineWeapon)
            {
                jetEngineWeapon.StartCharge();  // Start charging the weapon
            }
        }

        [ServerRpc]
        private void StopChargeServerRpc()
        {
            // Broadcast the charge stop to all clients
            StopChargeClientRpc();
        }

        [ClientRpc]
        private void StopChargeClientRpc()
        {
            if (Weapon is JetEngineWeapon jetEngineWeapon)
            {
                jetEngineWeapon.StopCharge();  // Stop charging the weapon
            }
        }
    }
}