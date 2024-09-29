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
                Backpack.AddWeapon(_startingWeapon);
                SyncWeapon(_startingWeapon.Type, OwnerClientId).Forget();
            }
        }

        public void AddWeapon(Weapon weapon)
        {
            if (!IsOwner) return;
            Backpack.AddWeapon(weapon);
            AddWeaponServerRpc(weapon.Type, OwnerClientId);
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

        // todo: check called twice for client on start
        private async UniTask SyncWeapon(WeaponType type, ulong ownerClientId)
        {
            await UniTask.WaitUntil(() => AppModel.PlayerTransform(ownerClientId) != null);

            var weaponSlot = AppModel.PlayerTransform(ownerClientId).Find("WeaponSlot");
            if(weaponSlot.childCount != 0)
                Destroy(weaponSlot.transform.GetChild(0).gameObject);
            
            var weaponPrefab = Backpack.GetWeapons().First(x => x.Type == type);

            var weaponInstance = Instantiate(weaponPrefab, weaponSlot, false);
            // todo: should be moved
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