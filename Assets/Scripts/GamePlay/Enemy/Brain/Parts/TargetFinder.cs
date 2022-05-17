namespace GamePlay.Enemy.Brain.Parts
{
  public class TargetFinder : BotPart
  {
    public TargetFinder(BotBrain brain) : base(brain)
    {
      brain.Target = AppModel.PlayerGameObj();
    }

    protected override void OnUpdate()
    {
      
    }
  }
}