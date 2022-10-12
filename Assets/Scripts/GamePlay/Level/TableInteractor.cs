using System;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class TableInteractor : Interactable
  {
    [SerializeField] private Table _table;
    private bool _isActive;

    public override void Interact(PlayerInteract playerInteract)
    {
      _table.Flip();
    }
    
    private void Update()
    {
      if (_isActive)
      {
        var posDif = AppModel.PlayerGameObj().transform.position - _table.transform.position;
        TableSide side;
        if (Mathf.Abs(posDif.y) < Mathf.Abs(posDif.x))
          side = posDif.x < 0 ? TableSide.Left : TableSide.Right;
        else
          side = posDif.y < 0 ? TableSide.Bot : TableSide.Top;
        _table.ChooseSide(side);
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        _isActive = true;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        _table.ChooseSide(null);
        _isActive = false;
      }
    }
  }
}