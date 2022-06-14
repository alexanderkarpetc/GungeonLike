﻿using GamePlay.Player;
using Import;
using UnityEngine;

namespace GamePlay
{
  public class SceneInitializer : MonoBehaviour
  {
    [SerializeField] private Color cameraColor = Color.black;
    [SerializeField] private GameObject HUD;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private bool _initplayer;

    void Awake()
    {
      InitCamera();
      InitContainers();
      BalanceLoader.LoadBalance();
      if(_initplayer)
        InitPlayer();
      else
      {
        AppModel.SetPlayer(GameObject.Find("Player"));
      }
      _playerController.Init();
      var straightLevelController = GameObject.Find("StraightLevelController");
      if(straightLevelController != null)
        DoStraightLevelLogic(straightLevelController.GetComponent<StraightLevelController>());
    }

    private void InitContainers()
    {
      var bullets = new GameObject("Bullets");
      var fxs = new GameObject("Fx");
      AppModel.SetContainers(bullets, fxs);
    }

    private void InitCamera()
    {
      var mainCamera = GameObject.Find("Main Camera");
      mainCamera.GetComponent<Camera>().backgroundColor = cameraColor;
      mainCamera.AddComponent<CameraFollow>();
      AppModel.InitHud(Instantiate(HUD).GetComponent<HudController>());
    }

    private void InitPlayer()
    {
      var playerGo = Instantiate(_playerController).gameObject;
      AppModel.SetPlayer(playerGo);
    }
    
    private void DoStraightLevelLogic(StraightLevelController straightLevelController)
    {
      straightLevelController.ProcessNextRoom();
    }
  }
}