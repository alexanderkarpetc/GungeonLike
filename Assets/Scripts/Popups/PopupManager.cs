using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Popups
{
  public class PopupManager : MonoBehaviour
  {
    public static PopupManager Instance;

    public List<Popup> _popupPrefabs;

    private List<Popup> _popups = new();

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
    }

    public T ShowPopup<T>() where T : Popup
    {
      var popupPrefab = _popupPrefabs.First(x => x.GetType() == typeof(T));
      var popupObj = Instantiate(popupPrefab, transform);
      _popups.Add(popupObj);
      return (T)popupObj;
    }

    public void CloseCurrentPopup()
    {
      if (_popups.Count > 0)
      {
        var popupObj = _popups.Last();
        _popups.Remove(popupObj);
        Destroy(popupObj.gameObject);
      }
    }

    public bool IsShown<T>()
    {
      return _popups.Any(x => x.GetType() == typeof(T));
    }

    public void HidePopup<T>()
    {
      var popupObj = _popups.First(x => x.GetType() == typeof(T));
      _popups.Remove(popupObj);
      Destroy(popupObj.gameObject);
    }
  }
}