using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public class SceneInitializer : MonoBehaviour
  {
    [SerializeField] private Color cameraColor = Color.black;
    [SerializeField] private GameObject HUD;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private PlayerController _player;
    [SerializeField] private bool _initplayer;

    void Awake()
    {
      InitCamera();
      if(_initplayer)
        InitPlayer();
      else
      {
        AppModel.SetPlayer(GameObject.Find("Player"));
      }
    }

    private void InitCamera()
    {
      var mainCamera = GameObject.Find("Main Camera");
      mainCamera.GetComponent<Camera>().backgroundColor = cameraColor;
      var cameraFollow = mainCamera.AddComponent<CameraFollow>();
      cameraFollow.Speed = cameraSpeed;
      Instantiate(HUD);
    }

    private void InitPlayer()
    {
      var playerGo = Instantiate(_player).gameObject;
      AppModel.SetPlayer(playerGo);
    }
  }
}