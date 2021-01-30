using System.Collections.Generic;
using System.Linq;
using GamePlay.Enemy;
using GamePlay.Weapons;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace GamePlay.Level
{
  public class EnemyFactory
  {
    public List<EnemySetup> EnemySetups = new List<EnemySetup>();
    public List<EnemySetup> BossSetups = new List<EnemySetup>();
    public EnemyFactory()
    {
      var enemies = Resources.LoadAll("Prefabs/Enemies", typeof(EnemyController))
        .Where(x => x.GetType() != typeof(BossController));
      foreach (EnemyController enemy in enemies)
      {
        EnemySetups.Add(new EnemySetup(enemy, enemy.Weapon?.GetPower() ?? 0));
      }
      var bosses = Resources.LoadAll("Prefabs/Enemies/Boss", typeof(EnemyController));
      foreach (EnemyController boss in bosses)
      {
        BossSetups.Add(new EnemySetup(boss, 100));
      }
    }

    public EnemySetup GetRandomEnemy()
    {
      var index = AppModel.random.NextInt(0, EnemySetups.Count);
      return EnemySetups[index];
    }
  }
}