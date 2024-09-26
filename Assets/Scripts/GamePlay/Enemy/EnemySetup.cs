using UnityEngine;

namespace GamePlay.Enemy
{
  public struct EnemySetup
  {
    public readonly EnemyController EnemyObject;
    public readonly int Power;
    public readonly EnemyType EnemyType;
    public readonly string Name;

    public EnemySetup(EnemyController enemyObject, int power, EnemyType enemyType)
    {
      EnemyObject = enemyObject;
      Power = power;
      EnemyType = enemyType;
      Name = enemyObject.name;
    }
  }
}