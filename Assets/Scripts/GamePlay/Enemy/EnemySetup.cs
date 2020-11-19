using UnityEngine;

namespace GamePlay.Enemy
{
  public struct EnemySetup
  {
    public readonly EnemyController EnemyObject;
    public readonly int Power;

    public EnemySetup(EnemyController enemyObject, int power)
    {
      EnemyObject = enemyObject;
      Power = power;
    }
  }
}