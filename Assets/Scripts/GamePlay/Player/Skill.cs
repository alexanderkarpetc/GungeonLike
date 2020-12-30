namespace GamePlay.Player
{
  public enum SkillKind
  {
    Gunslinger = 1, // Pistol damage
    SharpShooter = 2, //Automatic weapon firerate
  }
  public class Skill
  {
    public SkillKind Kind;
    public string Description;
    public string Icon;
    public SkillTreeBranchKind BranchKind;
    public float Impact;
  }
}