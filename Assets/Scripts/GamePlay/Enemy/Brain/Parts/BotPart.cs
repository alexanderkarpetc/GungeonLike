using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public abstract class BotPart : IUpdatable
  {
    protected BotBrain Brain;
    protected GameObject Owner => Brain.gameObject;
    private Dictionary<Action, float> _delayedActions = new Dictionary<Action, float>();

    protected BotPart(BotBrain brain)
    {
      Brain = brain;
    }
    
    protected void DelayCall(Action action, float delay)
    {
      _delayedActions.Add(action, Time.time + delay);
    }

    public void Update()
    {
      OnUpdate();
      if(_delayedActions.Count == 0)
        return;
      var actionToExecute = _delayedActions.FirstOrDefault(x => x.Value <= Time.time).Key;
      if(actionToExecute == null)
        return;
      actionToExecute.Invoke();
      _delayedActions.Remove(actionToExecute);
    }
    protected virtual void OnUpdate(){}
  }
}