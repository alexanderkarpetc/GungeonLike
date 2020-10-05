using UnityEngine;

namespace Player
{
    public class WeaponTurn : MonoBehaviour
    {
        void Update()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rot;
        }
    }
}
