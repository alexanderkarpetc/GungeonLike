using System.Linq;
using UnityEngine;

namespace GamePlay.Weapons
{
    public class LaserWeapon : Weapon
    {
        [SerializeField] private GameObject _laserSight;

        public void StartAim()
        {
            _laserSight.SetActive(true);
        }
        public void StopAim()
        {
            _laserSight.SetActive(false);
        }
        private void Update()
        {
            var direction = IsInverted
                ? DegreeToVector2(_shootPoint.rotation.eulerAngles.z) * new Vector2(-1, -1)
                : DegreeToVector2(_shootPoint.rotation.eulerAngles.z);
            var hit = Physics2D.RaycastAll(_shootPoint.position, direction)
                .First(x => !x.collider.CompareTag("Enemy") && 
                            !x.collider.CompareTag("Projectile"));
            var length = Vector2.Distance(hit.point, _shootPoint.position);
            _laserSight.transform.localScale = new Vector3(1, length*3 + 0.3f, 1);
        }
    }
}