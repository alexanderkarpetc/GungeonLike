using System.Collections;

namespace GamePlay.Enemy
{
  public class BossController : EnemyController
  {
    protected override IEnumerator HitAnimation()
    {
      yield return null;
    }
  }
}