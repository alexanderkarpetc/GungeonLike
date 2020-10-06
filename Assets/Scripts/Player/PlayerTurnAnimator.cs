using DefaultNamespace;
using UnityEngine;

namespace Player
{
  public class PlayerTurnAnimator : TurnAnimator
  {
    [HideInInspector] public int VerticalMove;
    [HideInInspector] public int HorizontalMove;
    private int _currentTurn;
    private bool _previousRunState;

    protected override void ProcessPlayerTurn(int direction, int scale)
    {
      var currentRunningState = HorizontalMove != 0 || VerticalMove != 0;
      if (_currentTurn == direction && _previousRunState == currentRunningState)
        return;
      _currentTurn = direction;
      _previousRunState = currentRunningState;
      TurnTo(direction, true);
      SpriteUtil.SetXScale(_body, scale);
    }
  }
}