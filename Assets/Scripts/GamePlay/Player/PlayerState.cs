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
        public NetworkVariable<WeaponType> CurrentWeaponType = new();

        public NetworkVariable<int> CurrentHp = new ();
        public NetworkVariable<int> MaxHp = new ();
        
        [SerializeField] private Weapon _startingWeapon;

        private int _skillsPoints;
        private int _level = 1;
        private int _exp = 0;
        private PlayerInitializer _initializer = new PlayerInitializer();

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
            else if(!IsServer)
            {
                // here client receives server gun
                SyncWeapon(CurrentWeaponType.Value, OwnerClientId).Forget();
            }

            if (IsServer)
            {
                MaxHp.Value = 100;
                CurrentHp.Value = 100;
            }
        }

        public void AddWeapon(Weapon weapon)
        {
            if (!IsOwner) return;
            if (Backpack.GetWeapons().Count >= StaticData.BackpackCapacity)
            {
                DropGunServerRpc(transform.position, CurrentWeaponType.Value);
                Backpack.RemoveCurrentWeapon();
            }
            Backpack.AddWeapon(weapon);
            AddWeaponServerRpc(weapon.Type, OwnerClientId);
        }

        [ServerRpc]
        public void DropGunServerRpc(Vector3 pos, WeaponType type)
        {
            AppModel.DropManager().DropGun(pos, type);
        }

        [ServerRpc]
        public void AddWeaponServerRpc(WeaponType type, ulong ownerClientId)
        {
            CurrentWeaponType.Value = type;
            SyncWeaponToClientsClientRpc(type, ownerClientId);
        }

        [ClientRpc]
        public void SyncWeaponToClientsClientRpc(WeaponType type, ulong ownerClientId)
        {
            SyncWeapon(type, ownerClientId).Forget();
        }

        private async UniTask SyncWeapon(WeaponType type, ulong ownerClientId)
        {
            await UniTask.WaitUntil(() => AppModel.PlayerTransform(ownerClientId) != null);

            var weaponSlot = AppModel.PlayerTransform(ownerClientId).Find("WeaponSlot");
            if(weaponSlot.childCount != 0)
                Destroy(weaponSlot.transform.GetChild(0).gameObject);
            
            var weaponPrefab = AppModel.DropManager().AllGuns.First(x => x.Type == type);

            var weaponInstance = Instantiate(weaponPrefab, weaponSlot, false);
            // todo: need to remember bullets left
            weaponInstance.State = new WeaponState { bulletsLeft = weaponInstance.MagazineSize };
            weaponInstance.IsPlayers = true;

            var playerWeaponTurn = GetComponent<PlayerWeaponTurn>();
            playerWeaponTurn.Weapon = weaponInstance;
            
            weaponInstance.IsOwner = IsOwner;
            _weapon = weaponInstance;
        }

        public void NextWeapon()
        {
            if (!IsOwner) return;
            Backpack.NextWeapon(); 
            AddWeaponServerRpc(Backpack.CurrentWeapon.Type, OwnerClientId);
        }

        public void Heal(int value)
        {
            CurrentHp.Value = Mathf.Clamp(CurrentHp.Value + value, 0, MaxHp.Value);
        }

        public void DealDamage(int value)
        {
            CurrentHp.Value -= value;
            OnDamageTake?.Invoke();
        }

        public void IncreaseMaxHp(int value)
        {
            MaxHp.Value += value;
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