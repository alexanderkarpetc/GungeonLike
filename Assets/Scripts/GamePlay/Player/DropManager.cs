using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Extensions;
using GamePlay.Level;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class DropManager
  {
    public List<Weapon> AllGuns = new List<Weapon>(); 
    private PickableItemView _pedestal;
    private PickableItemView _ammoBox;
    private PickableItemView _coin;
    private GameObject _parentObj;

    public DropManager()
    {
      var guns = Resources.LoadAll("Prefabs/Guns", typeof(Weapon));
      foreach (var gun in guns)
      {
        AllGuns.Add(gun as Weapon);
      }
      _pedestal = Resources.Load("Prefabs/Player/Pedestal", typeof(PickableItemView)) as PickableItemView;
      _ammoBox = Resources.Load("Prefabs/Player/AmmoBox", typeof(PickableItemView)) as PickableItemView;
      _coin = Resources.Load("Prefabs/Player/Resource", typeof(PickableItemView)) as PickableItemView;
    }

    public void DropOnEnemyDeath(Transform transform, EnemyType enemyType)
    {
      if(_parentObj == null)
        _parentObj = Util.InitParentIfNeed("Drop");
      if ((int)enemyType >= 100)
      {
        var pedestal = Object.Instantiate(_pedestal, transform.position, Quaternion.identity, _parentObj.transform);
        pedestal.Weapon = AllGuns.First();
      }
      else
      {
        var pedestal = Object.Instantiate(_ammoBox, transform.position, Quaternion.identity, _parentObj.transform);
        var deficientAmmo = FindDeficientAmmo(3);
        pedestal.Ammo = new Dictionary<AmmoKind, int>
        {
          {deficientAmmo[0], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[0])},
          {deficientAmmo[1], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[1])},
          {deficientAmmo[2], AppModel.WeaponData().GetAmmoAmountForKind(deficientAmmo[2])},
        };
      }
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

    public Transform GetDropped()
    {
      return _parentObj.transform;
    }
  }
}