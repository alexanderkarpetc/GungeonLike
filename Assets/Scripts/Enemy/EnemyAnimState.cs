using UnityEngine;

namespace Enemy
{
  public class EnemyAnimState
  {
    public static readonly int die = Animator.StringToHash("die");
    public static readonly int upRight = Animator.StringToHash("upRight");
    public static readonly int upLeft = Animator.StringToHash("upLeft");
    public static readonly int downRight = Animator.StringToHash("downRight");
    public static readonly int downLeft = Animator.StringToHash("downLeft");
    public static readonly int idle = Animator.StringToHash("idle");
    public static readonly int idleBack = Animator.StringToHash("idleBack");
    public static readonly int hitLeftUp = Animator.StringToHash("hitLeftUp");
    public static readonly int hitLeftDown = Animator.StringToHash("hitLeftDown");
    public static readonly int hitRightUp = Animator.StringToHash("hitRightUp");
    public static readonly int hitRightDown = Animator.StringToHash("hitRightDown");

  }
}