using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerWeaponSelect : MonoBehaviour
  {
    void Update()
    {
      if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
      {
        AppModel.PlayerState().Backpack.NextWeapon();
      }
      else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
      {
        AppModel.PlayerState().Backpack.PreviousWeapon();
      }

      if (Input.GetKeyDown(KeyCode.R)) // backwards
      {
        AppModel.PlayerState().Weapon.Reload();
      }
    }
  }
}