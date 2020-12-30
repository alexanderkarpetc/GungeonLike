using System.Linq;
using GamePlay.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Player
{
  public class Console : MonoBehaviour
  {
    [SerializeField] private InputField _inputField;
    [SerializeField] private Weapon[] _weapons;

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return))
      {
        ApplyCommand();
      }
    }

    private void ApplyCommand()
    {
      var text = _inputField.text.Split(' ');
      switch (text[0].ToLower())
      {
        case "addgun":
          TryAddGun(text[1]);
          break;
        default:
          Debug.LogError("No SUCH command " + text[0]);
          break;
      }

      Debug.Log(text);
      _inputField.text = "";
    }

    private void TryAddGun(string name)
    {
      var weapon = _weapons.FirstOrDefault(x => x.name.Equals(name));
      if (weapon == null)
        weapon = _weapons.First(x => x.name.ToLower().Contains(name.ToLower()));
      AppModel.Player().Backpack.AddWeapon(weapon);
    }
  }
}