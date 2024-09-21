namespace GamePlay.Weapons
{
    public class EnergyWeapon : Weapon
    {
        protected override void Start()
        {
            State = new WeaponState{bulletsLeft = AppModel.PlayerState().Backpack.Ammo[AmmoKind]};
            BaseDamage  = AppModel.WeaponData().GetWeaponInfo(Type).Damage;
        }
    }
}