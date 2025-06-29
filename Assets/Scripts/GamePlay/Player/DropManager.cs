using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Extensions;
using GamePlay.Level;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GamePlay.Player
{
  public class DropManager
  {
    public List<Weapon> AllGuns = new List<Weapon>(); 
    private AutoPickableItemView _pedestal;
    private AutoPickableItemView _ammoBox;
    private AutoPickableItemView _coin;
    private List<AutoPickableItemView> _drops = new();

    public List<AutoPickableItemView> GetDropped => _drops.Where(x => x != null).ToList();
    public DropManager()
    {
      var guns = Resources.LoadAll("Prefabs/Guns", typeof(Weapon));
      foreach (var gun in guns)
      {
        AllGuns.Add(gun as Weapon);
      }
      _pedestal = Resources.Load("Prefabs/Player/Pedestal", typeof(AutoPickableItemView)) as AutoPickableItemView;
      _ammoBox = Resources.Load("Prefabs/Player/AmmoBox", typeof(AutoPickableItemView)) as AutoPickableItemView;
      _coin = Resources.Load("Prefabs/Player/Resource", typeof(AutoPickableItemView)) as AutoPickableItemView;
    }

    public void DropOnEnemyDeath(Transform transform, EnemyType enemyType)
    {
      AutoPickableItemView pedestal;
      if ((int)enemyType >= 100)
      {
        throw new NotImplementedException("Not implemented");
        // pedestal = Object.Instantiate(_pedestal, transform.position, Quaternion.identity);
        // pedestal.GetComponent<NetworkObject>().Spawn();
        // pedestal.SetWeaponServerRpc(WeaponType.Crossbow);
      }
      else
      {
        pedestal = Object.Instantiate(_ammoBox, transform.position, Quaternion.identity);
        pedestal.GetComponent<NetworkObject>().Spawn();
        var deficientAmmo = FindDeficientAmmo(1);
        pedestal.AddAmmoServerRpc(deficientAmmo[0], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[0]));
        // todo: add  more ammo types like it was before
        // pedestal.Ammo = new Dictionary<AmmoKind, int>
        // {
        //   {deficientAmmo[0], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[0])},
        //   {deficientAmmo[1], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[1])},
        //   {deficientAmmo[2], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[2])},
        // };
      }
      _drops.Add(pedestal);
    }

    public Weapon GetAbsentWeapon()
    {
      return AllGuns.Where(x => !AppModel.PlayerState().Backpack.GetWeapons().Contains(x)).ToList().Random();
    }

    private List<AmmoKind> FindDeficientAmmo(int quantity)
    {
      var kindToPercent = new Dictionary<AmmoKind, float>();
      foreach (var ammo in AppModel.PlayerState().Backpack.Ammo)
      {
        kindToPercent.Add(ammo.Key, (float) ammo.Value / AppModel.WeaponData().AmmoCapacity[ammo.Key]);
      }

      var percentagesList = kindToPercent.ToList();
      percentagesList.Sort((x,y)=> x.Value.CompareTo(y.Value));

      return percentagesList.Select(x=>x.Key).Take(quantity).ToList();
    }
  }
}