using System.Collections.Generic;
using GamePlay.Level;
using GamePlay.Level.Controllers;
using GamePlay.UI;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay
{
    public class StraightLevelController : NetworkBehaviour
    {
        // todo: randomize later and sync
        [SerializeField] private List<GameObject> predefinedRooms;
        [SerializeField] private CameraFade _cameraFade;
        [SerializeField] private RoomData _currentRoom;
        [SerializeField] private GameObject _astarObj;
        [SerializeField] private RoomController _roomController;

        private int _currentRoomIndex = -1;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Init();
            }
        }

        public void Init()
        {
            AppModel.StraightRoomController = this;
            if(_currentRoom != null)
            {
                _roomController.Init();
                _roomController.Set(_currentRoom, true);
            }
        }

        public void ProcessNextRoom()
        {
            if (IsServer)
            {
                ProcessNextRoomServerRpc();
            }
        }

        [ServerRpc]
        private void ProcessNextRoomServerRpc()
        {
            // Fade the camera on all clients
            _cameraFade.MakeFade();

            _currentRoomIndex++;

            _roomController.DespawnEnv();

            foreach (var item in AppModel.DropManager().GetDropped)
            {
                var itemNetworkObject = item.GetComponent<NetworkObject>();
                if (itemNetworkObject != null && itemNetworkObject.IsSpawned)
                {
                    itemNetworkObject.Despawn();
                }
            }
            NextRoomClientRpc(_currentRoomIndex);
            UpdatePlayerPositionClientRpc(_currentRoom.transform.Find("StartPoint").position);
        }

        [ClientRpc]
        private void UpdatePlayerPositionClientRpc(Vector3 startPoint)
        {
            // Update the player position on each client
            AppModel.PlayerTransform().position = startPoint;
        }

        public void ReInitAstar()
        {
            if (IsServer)
            {
                ReInitAstarServerRpc();
            }
        }

        [ServerRpc]
        private void ReInitAstarServerRpc()
        {
            if (_astarObj == null)
            {
                var astarPrefab = Resources.Load<GameObject>("Prefabs/Util/AStar");
                _astarObj = Instantiate(astarPrefab);

                var astarNetworkObject = _astarObj.GetComponent<NetworkObject>();
                if (astarNetworkObject != null)
                {
                    astarNetworkObject.Spawn();
                }
            }
            else
            {
                _astarObj.GetComponent<AstarPath>().Scan();
            }
        }
        
        [ClientRpc]
        private void NextRoomClientRpc(int roomIndex)
        {
            Destroy(_currentRoom.gameObject);
            var nextRoomPrefab = predefinedRooms[roomIndex];
            _currentRoom = Instantiate(nextRoomPrefab).GetComponent<RoomData>();

            if(IsServer)
                _roomController.Set(_currentRoom, false);
        }
    }
}