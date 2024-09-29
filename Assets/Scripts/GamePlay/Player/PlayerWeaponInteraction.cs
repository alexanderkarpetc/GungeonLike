using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerWeaponInteraction : NetworkBehaviour
  {
    
    private void Update()
    {
      if(!IsOwner)
        return;
      if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
      {
        AppModel.PlayerState().NextWeapon();
      }
      else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
      {
        // todo: implement PreviousWeapon
        // AppModel.PlayerState().PreviousWeapon();
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
        StartReloadServerRpc(OwnerClientId);
      }
    }
    
    [ServerRpc]
    private void StartReloadServerRpc(ulong ownerClientId, ServerRpcParams serverRpcParams = default)
    {
      StartReloadClientRpc(ownerClientId);
    }
    
    [ClientRpc]
    private void StartReloadClientRpc(ulong ownerClientId, ClientRpcParams clientRpcParams = default)
    {
      AppModel.PlayerState(ownerClientId).Weapon.Reload();
    }
  }
}