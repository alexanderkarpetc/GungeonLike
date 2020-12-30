using System.Collections.Generic;
using GamePlay.Player;

namespace GamePlay.Common
{
  public static class StaticData
  {
    public static float PlayerSpeedBase = 7;
    public static float EnemyBulletSpeedBase = 2;
    public static float EnemyCubulonSpeedBase = 5;
    public static int EnemyCubulonShotsCount = 30;
    public static float EnemyCubulonShotSpeed = 7;
    
    
    public static List<Skill> Skills;

    public static int RequiredXp(int level)
    {
      return 1000;
    }
  }
}