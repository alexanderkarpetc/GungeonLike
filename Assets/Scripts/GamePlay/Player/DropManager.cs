using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GamePlay.Level;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class DropManager
  {
    public List<Weapon> AllGuns = new List<Weapon>(); 
    private Pedestal _pedestalPref;
    private GameObject _parentObj;

    public DropManager()
    {
      var guns = Resources.LoadAll("Prefabs/Guns", typeof(Weapon));
      foreach (var gun in guns)
      {
        AllGuns.Add(gun as Weapon);
      }
      _pedestalPref = Resources.Load("Prefabs/Pedestal", typeof(Pedestal)) as Pedestal;
    }

    public void CheckDrop(Transform transform, int importance)
    {
      if(_parentObj == null)
        _parentObj = Util.InitParentIfNeed("Drop");
      var pedestal = Object.Instantiate(_pedestalPref, transform.position, Quaternion.identity, _parentObj.transform);
      pedestal.Item = AllGuns.First();
    }

    public Transform GetDropped()
    {
      return _parentObj.transform;
    }
  }
}