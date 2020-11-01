using GamePlay.Player;

namespace GamePlay
{
  public static class AppModel
  {
    private static PlayerState _playerState;

    public static PlayerState Player()
    {
      return _playerState ?? (_playerState = new PlayerState());
    }
  }
}