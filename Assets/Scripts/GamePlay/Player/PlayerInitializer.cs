using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInitializer
  {
    public void Init(Weapon startingWeapon)
    {
      AppModel.Player().AddSkillPoint();
      AppModel.Player().AddSkillPoint();
      AppModel.Player().AddSkillPoint();
      AppModel.Player().Backpack.AddWeapon(startingWeapon);
    }
  }
}