using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Level;
using UnityEditor.Animations;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerKick : MonoBehaviour
  {
    [SerializeField] private float _kickDuration;
    [SerializeField] private float _kickPower;
    [SerializeField] private PlayerAnimatorView _animatorView;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerInteract _playerInteract;

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.F) && !_controller.IsBusy)
      {
        StartCoroutine(DoKick());
      }
    }

    private IEnumerator DoKick()
    {
      _controller.IsBusy = true;
      _animatorView.IsKicking = true;
      
      var interactable = _playerInteract.GetInteractable();
      if (interactable != null && interactable is PickableInteractable pickable)
      {
        var hitDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var lookDirection = _playerInteract.GetPlayerLookDirection();
        pickable.Kick(hitDirection.normalized, _kickPower, lookDirection);
      }
      yield return new WaitForSeconds(_kickDuration);
      _controller.IsBusy = false;
      _animatorView.IsKicking = false;
    }
  }
}