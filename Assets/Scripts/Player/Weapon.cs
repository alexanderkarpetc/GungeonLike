using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
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
                Instantiate(_projectile, _shootPoint.position, transform.rotation);
                _nextShotTime = Time.time + _shootRate;
            }
        }
    }
}
