using System;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level
{
  public class DeveloperView : NetworkBehaviour
  {
    private GameObject _devPanel;
    private bool _leftControl;

    private void Start()
    {
      _devPanel = AppModel.Hud().DevPanel;
    }

    private void Update()
    {
      if(!IsOwner) return;
      if(Input.GetKeyDown(KeyCode.LeftControl))
        _leftControl = true;
      if(Input.GetKeyUp(KeyCode.LeftControl))
        _leftControl = false;
      if (_leftControl && Input.GetKeyDown(KeyCode.Q))
      {
        _devPanel.SetActive(!_devPanel.activeSelf);
      }
    }
  }
}
