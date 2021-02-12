using UnityEngine;

namespace GamePlay
{
  public class AnimationAutoDestroy : MonoBehaviour
  {
    void Start()
    {
      Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
  }
}