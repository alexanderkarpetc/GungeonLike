using System.Collections.Generic;
using System.Linq;
using GamePlay.Level;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class DropManager
  {
    public List<Weapon> AllGuns = new List<Weapon>(); 
    private Pedestal _pedestalPref; 
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
      Debug.Log(importance);
      var pedestal = GameObject.Instantiate(_pedestalPref, transform.position, Quaternion.identity);
      pedestal.Item = AllGuns.First();
    }
  }
}