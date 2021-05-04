using System.Collections.Generic;

namespace GamePlay.Extensions
{
  public static class ListExtensions
  {
    
    public static T Random<T>(this List<T> list)
    {
      var i = AppModel.random.NextInt(0, list.Count);
      return list[i];
    }
  }
}