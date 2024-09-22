using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerWeaponSelect : MonoBehaviour
  {
    private void Update()
    {
      if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
      {
        AppModel.PlayerState().NextWeapon();
      }
      else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
      {
        // todo: implement PreviousWeapon
        // AppModel.PlayerState().PreviousWeapon();
      }

      if (Input.GetKeyDown(KeyCode.R)) // backwards
      {
        AppModel.PlayerState().Weapon.Reload();
      }
    }
  }
}