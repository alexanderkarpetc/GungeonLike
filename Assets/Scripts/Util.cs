using UnityEngine;

namespace DefaultNamespace
{
  public static class Util
  {
    public static GameObject InitParentIfNeed(string name)
    {
      var levelParent = GameObject.Find(name);
      if (levelParent == null)
      {
        levelParent = new GameObject(name);
      }

      return levelParent;
    }
  }
}