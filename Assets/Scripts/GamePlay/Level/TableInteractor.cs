using System;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class TableInteractor : MonoBehaviour
  {
    [SerializeField] private Table _table;
    private TableInteractable _interactable;
    private bool _isActive;

    private void Start()
    {
      _interactable = new TableInteractable {Table = _table};
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
        _interactable.Table.ChooseSide(side);
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        _isActive = true;
        other.GetComponent<PlayerInteract>().Interactable = _interactable;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        _isActive = false;
        _interactable.Table.ChooseSide(null);
        other.GetComponent<PlayerInteract>().Interactable = null;
      }
    }
  }
}