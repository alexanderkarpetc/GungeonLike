using GamePlay.Common;
using GamePlay.Player;
using Import;
using Popups;
using UnityEngine;

namespace GamePlay
{
  public class SceneInitializer : MonoBehaviour
  {
    [SerializeField] private Color cameraColor = Color.black;
    [SerializeField] private GameObject HUD;
    [SerializeField] private PopupManager _popupManager;

    private void Awake()
    {
      InitCamera();
      InitContainers();
      BalanceLoader.LoadBalance();
      // todo: check if needed
      var straightLevelController = GameObject.Find("StraightLevelController");
      if(straightLevelController != null)
        DoStraightLevelLogic(straightLevelController.GetComponent<StraightLevelController>());
    }

    private void InitContainers()
    {
      var bullets = new GameObject("Bullets");
      bullets.AddComponent<BulletPoolManager>();
      var fxs = new GameObject("Fx");
      var env = GameObject.Find("Env");
      if(env == null)
        env = new GameObject("Env");
      AppModel.SetContainers(bullets, fxs, env);
      Instantiate(_popupManager);
    }

    private void InitCamera()
    {
      var mainCamera = GameObject.Find("Main Camera");
      mainCamera.GetComponent<Camera>().backgroundColor = cameraColor;
      mainCamera.AddComponent<CameraFollow>();
      AppModel.InitHud(Instantiate(HUD).GetComponent<HudController>());
    }

    private void DoStraightLevelLogic(StraightLevelController straightLevelController)
    {
      straightLevelController.Init();
      straightLevelController.ProcessNextRoom();
    }
  }
}