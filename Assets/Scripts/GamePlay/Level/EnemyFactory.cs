using System.Collections.Generic;
using GamePlay.Enemy;
using GamePlay.Weapons;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace GamePlay.Level
{
  public class EnemyFactory
  {
    public List<EnemySetup> EnemySetups = new List<EnemySetup>();
    public EnemyFactory()
    {
      var enemies = Resources.LoadAll("Prefabs/Enemies", typeof(EnemyController));
      foreach (EnemyController enemy in enemies)
      {
        EnemySetups.Add(new EnemySetup(enemy, enemy.Weapon.GetPower()));
      }
    }

    public EnemySetup GetRandomEnemy()
    {
      var index = AppModel.random.NextInt(0, EnemySetups.Count - 1);
      return EnemySetups[index];
    }
  }
}