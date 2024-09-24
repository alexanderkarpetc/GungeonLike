using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerState : NetworkBehaviour
    {
        // todo: not a good idea to have this here
        public NetworkVariable<WeaponType> CurrentWeaponType = new NetworkVariable<WeaponType>();

        public int CurrentHp;
        public int MaxHp;
        
        [SerializeField] private Weapon _startingWeapon;

        private int _skillsPoints;
        private int _level = 1;
        private int _exp = 0;
        private PlayerInitializer _initializer = new PlayerInitializer();

        public event Action OnHealthChanged;
        public event Action OnDamageTake;
        public Weapon Weapon => _weapon;
        public Backpack Backpack = new Backpack(); 
        public float SpeedMultiplier = 1f;
        public List<Skill> Skills = new List<Skill>();
        public int Level => _level;
        public int Exp => _exp;
        
        private Weapon _weapon;

        public event Action OnSkillLearned;

        private void Start()
        {
            AppModel.SetPlayer(this, OwnerClientId);
            if (IsOwner)
            {
                gameObject.name = $"PlayerOwner_{OwnerClientId}";
                AppModel.SetOwner(OwnerClientId);
                _initializer.Init(_startingWeapon);
            }
            else
            {
                gameObject.name = $"Player_{OwnerClientId}";
                // todo: VERY dirty shit
                if(!IsHost)
                    SyncWeaponWithWait(_startingWeapon.Type, OwnerClientId).Forget();
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            if (!IsOwner) return;

            _weapon = weapon;
            Backpack.AddWeapon(weapon);
            AddWeaponServerRpc(weapon.Type, OwnerClientId);
        }

        [ServerRpc]
        public void AddWeaponServerRpc(WeaponType type, ulong ownerClientId)
        {
            SyncWeaponToClientsClientRpc(type, ownerClientId);
            CurrentWeaponType.Value = type;
        }

        [ClientRpc]
        public void SyncWeaponToClientsClientRpc(WeaponType type, ulong ownerClientId)
        {
            SyncWeaponWithWait(type, ownerClientId).Forget();
        }

        private async UniTask SyncWeaponWithWait(WeaponType type, ulong ownerClientId)
        {
            await UniTask.WaitUntil(() => AppModel.PlayerTransform(ownerClientId) != null);
            // Find the weapon prefab on the client (same way as on the server)
            var weaponPrefab = AppModel.DropManager().AllGuns.First(x => x.Type == type);

            // Instantiate the weapon on the client side
            var weaponInstance = Instantiate(weaponPrefab);

            // Set the weapon as a child of the WeaponSlot (client-side)
            var weaponSlot = AppModel.PlayerTransform(ownerClientId).Find("WeaponSlot");
            weaponInstance.transform.SetParent(weaponSlot, false);  // Set parent without affecting local scale/position

            var playerWeaponTurn = GetComponent<PlayerWeaponTurn>();
            playerWeaponTurn.Weapon = weaponInstance;
            
            Backpack.AddWeapon(weaponInstance);
        }

        // Weapon switching
        public void NextWeapon()
        {
            if (!IsOwner) return; // Only the owning client can call this
            Backpack.NextWeapon(); // Update locally
            SyncWeaponChangeServerRpc(Backpack.CurrentWeaponIndex); // Notify server to sync change
        }

        // Server-side: sync weapon change
        [ServerRpc]
        public void SyncWeaponChangeServerRpc(int weaponIndex)
        {
            SyncWeaponChangeClientRpc(weaponIndex); // Sync change with all clients
            CurrentWeaponType.Value = Backpack.GetWeapons()[Backpack.CurrentWeaponIndex].Type;
        }

        // Client-side: sync backpack state
        [ClientRpc]
        public void SyncWeaponChangeClientRpc(int weaponIndex)
        {
            Backpack.SelectWeapon(weaponIndex); // Update backpack on clients
        }

        public void Heal()
        {
            CurrentHp = Mathf.Clamp(CurrentHp + 1, 0, MaxHp);
            OnHealthChanged?.Invoke();
        }

        public void DealDamage()
        {
            CurrentHp--;
            OnHealthChanged?.Invoke();
            OnDamageTake?.Invoke();
        }

        public void IncreaseMaxHp()
        {
            MaxHp++;
            OnHealthChanged?.Invoke();
        }

        public void LearnSkill(Skill skill)
        {
            _skillsPoints--;
            Skills.Add(skill);
            OnSkillLearned?.Invoke();
        }

        public int GetSkillPoints()
        {
            return _skillsPoints;
        }

        public void AddSkillPoint()
        {
            _skillsPoints++;
        }

        public Skill GetNextAvailableSkillOfKind(SkillTreeBranchKind kind)
        {
            var skillsOfKind = StaticData.Skills.Where(x => x.BranchKind == kind).ToList();
            return skillsOfKind.First(x => !Skills.Contains(x));
        }

        private void LevelUp()
        {
            _level++;
            _skillsPoints++;
        }

        public void AddExp(int exp)
        {
            _exp += exp;
            if (_exp >= StaticData.RequiredXp(_level))
            {
                _exp -= StaticData.RequiredXp(_level);
                LevelUp();
            }
        }

        public void AddExp(EnemyType type)
        {
            AddExp(100);
        }
    }
}