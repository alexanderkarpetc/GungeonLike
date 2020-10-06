using UnityEngine;

namespace DefaultNamespace
{
  public static class SpriteUtil
  {
    
    public static void SetXScale(GameObject go, int x)
    {
      var bodyTransform = go.transform;
      var newScale = bodyTransform.localScale;
      newScale.x = x;
      bodyTransform.localScale = newScale;
    }
  }
}