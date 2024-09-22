using System;
using System.Collections.Generic;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
    public class Backpack
    {
        public event Action<Dictionary<AmmoKind, int>> OnAmmoChange;
        public event Action<ResourceKind, int> OnResourcesChange;
        public bool IsInitialized;
        public int CurrentWeaponIndex = -1;
        public Dictionary<AmmoKind, int> Ammo = new Dictionary<AmmoKind, int>();
        
        private Dictionary<ResourceKind, int> _resources;
        private readonly List<Weapon> _weapons = new List<Weapon>();

        public Dictionary<ResourceKind, int> Resources
        {
            get => _resources;
            set
            {
                _resources = value;
                foreach (var valuePair in _resources)
                {
                    OnResourcesChange?.Invoke(valuePair.Key, valuePair.Value);
                }
            }
        }

        public int GetCoins()
        {
            return _resources[ResourceKind.Coins];
        }

        public List<Weapon> GetWeapons()
        {
            return new List<Weapon>(_weapons);
        }

        public void AddWeapon(Weapon newWeapon)
        {
            _weapons.Add(newWeapon);
            CurrentWeaponIndex = _weapons.Count - 1;
            newWeapon.State = new WeaponState { bulletsLeft = newWeapon.MagazineSize };
            newWeapon.IsPlayers = true;
        }

        public void SelectWeapon(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= _weapons.Count)
                return;

            CurrentWeaponIndex = weaponIndex;
            // Logic to switch weapon (handled in PlayerState)
            // var selectedWeapon = _weapons[CurrentWeaponIndex];
        }

        public void NextWeapon()
        {
            if (_weapons.Count <= 1) return;
            CurrentWeaponIndex = (CurrentWeaponIndex + 1) % _weapons.Count;
            SelectWeapon(CurrentWeaponIndex);
        }

        public void PreviousWeapon()
        {
            if (_weapons.Count <= 1) return;
            CurrentWeaponIndex = (CurrentWeaponIndex == 0) ? _weapons.Count - 1 : CurrentWeaponIndex - 1;
            SelectWeapon(CurrentWeaponIndex);
        }

        public void AddAmmo(Dictionary<AmmoKind, int> ammo)
        {
            foreach (var pair in ammo)
            {
                Ammo[pair.Key] = Mathf.Clamp(Ammo.GetValueOrDefault(pair.Key) + pair.Value, 0, 999); // or use a max limit
            }

            OnAmmoChange?.Invoke(ammo);
        }

        public void AddResource(ResourceKind kind, int amount)
        {
            Resources[kind] = Resources.GetValueOrDefault(kind) + amount;
            OnResourcesChange?.Invoke(kind, amount);
        }

        public void WithdrawResource(ResourceKind kind, int amount)
        {
            Resources[kind] = Resources.GetValueOrDefault(kind) - amount;
            OnResourcesChange?.Invoke(kind, amount);
        }
    }
}