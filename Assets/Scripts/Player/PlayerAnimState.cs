using UnityEngine;

namespace Player
{
  public class PlayerAnimState
  {
    public static readonly int DownRight = Animator.StringToHash("downRight");
    public static readonly int UpRight = Animator.StringToHash("upRight");
    public static readonly int Up = Animator.StringToHash("up");
    public static readonly int Down = Animator.StringToHash("down");
    public static readonly int UpRun = Animator.StringToHash("upRun");
    public static readonly int DownRun = Animator.StringToHash("downRun");
    public static readonly int DownRightRun = Animator.StringToHash("downRightRun");
    public static readonly int UpRightRun = Animator.StringToHash("upRightRun");
    
    public static readonly string IdleDown = "IdleDown";
    public static readonly string IdleUp = "IdleUp";
    public static readonly string IdleDownRight = "IdleDownRight";
    public static readonly string IdleUpRight = "IdleUpRight";
    public static readonly string RunUp = "RunUp";
    public static readonly string RunUpRight = "RunUpRight";
    public static readonly string RunDown = "RunDown";
    public static readonly string RunDownRight = "RunDownRight";
  }
}