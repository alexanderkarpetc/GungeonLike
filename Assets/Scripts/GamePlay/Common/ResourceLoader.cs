using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Common
{
    public class ResourceLoader
    {
        private static ResourceLoader _instance;
        private GameObject _blueProjectile;
        private string _blueProjectileName;

        public static ResourceLoader Instance => _instance ??= new ResourceLoader();

        public void Preload()
        {
            _blueProjectile = Resources.Load<GameObject>("Prefabs/Projectiles/BlueProjectile");
            _blueProjectileName = _blueProjectile.GetComponent<Projectile>().ProjectileName;
        }
        public void GetCubulonResources(out GameObject proj, out string projName)
        {
            proj = _blueProjectile;
            projName = _blueProjectileName;
        }
        public void GetGunKnightResources(out GameObject proj, out string projName)
        {
            proj = _blueProjectile;
            projName = _blueProjectileName;
        }
    }
}