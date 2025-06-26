using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
    public class CubulonBrain : BotBrain
    {
        public override void Init()
        {
            base.Init();
            _parts.Clear();
            var cubulonShooting = new CubulonBotShooting(this);
            var cubulonMoving = new PatrolBotMoving(this);
            _parts.Add(cubulonShooting);
            _parts.Add(cubulonMoving);
            EnemyController.GetAiPath().maxSpeed = StaticData.EnemyCubulonSpeedBase;
        }

        [ClientRpc]
        protected override void ShootClientRpc()
        {
            var angleShift = 360 / StaticData.EnemyCubulonShotsCount;
            for (var i = 0; i < StaticData.EnemyCubulonShotsCount; i++)
            {
                ResourceLoader.Instance.GetCubulonResources(out var proj, out var projName);

                var go = BulletPoolManager.Instance.GetBulletFromPool(proj, transform.position, Quaternion.identity, projName);
                go.transform.SetParent(AppModel.BulletContainer().transform);
                var projectile = go.GetComponent<Projectile>();
                projectile.Speed = StaticData.EnemyCubulonShotSpeed;
                projectile.Direction = Weapon.DegreeToVector2(i * angleShift);
                projectile.Damage = 2;
                projectile.IsPlayerBullet = false;
                projectile.IsOwner = true;
            }
        }
    }
}