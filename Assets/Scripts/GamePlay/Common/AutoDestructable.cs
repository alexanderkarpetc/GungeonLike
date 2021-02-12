using UnityEngine;

namespace GamePlay
{
  public class AutoDestructable : MonoBehaviour
  {
    [SerializeField] private float seconds;

    void Start()
    {
      Destroy(gameObject, seconds);
    }
  }
}