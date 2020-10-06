using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        public bool IsInverted;

        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;
        [SerializeField] private float _shootRate;
        [SerializeField] private float _reloadTime;
        
        
         private float _nextShotTime;
         private bool _reloading;

         // Update is called once per frame
        void Update()
        {
            CheckShoot();
        }

        private void CheckShoot()
        {
            if(Input.GetMouseButton(0) && Time.time>=_nextShotTime && !_reloading)
            {
                var go = Instantiate(_projectile, _shootPoint.position, transform.rotation);
                var projectile = go.GetComponent<Projectile>();
                projectile.IsInverted = IsInverted;
                _nextShotTime = Time.time + _shootRate;
            }
        }
    }
}
