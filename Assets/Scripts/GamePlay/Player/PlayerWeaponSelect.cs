using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerWeaponSelect : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
            {
                AppModel.Player().Backpack.NextWeapon();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
            {
                AppModel.Player().Backpack.PreviousWeapon();
            }
        }
    }
}
