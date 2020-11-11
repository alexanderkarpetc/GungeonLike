namespace GamePlay.Weapons
{
  public class WeaponState
  {
    public int bulletsLeft;
    public override string ToString()
    {
      return $"Bullets left:{bulletsLeft}";
    }
  }
}