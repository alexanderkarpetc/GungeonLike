using UnityEngine;

namespace GamePlay
{
  public class SceneInitializer : MonoBehaviour
  {
    [SerializeField] private Color cameraColor = Color.black;
    [SerializeField] private float cameraSpeed;

    void Start()
    {
      var mainCamera = GameObject.Find("Main Camera");
      mainCamera.GetComponent<Camera>().backgroundColor = cameraColor;
      var cameraFollow = mainCamera.AddComponent<CameraFollow>();
      cameraFollow.Speed = cameraSpeed;
    }
  }
}