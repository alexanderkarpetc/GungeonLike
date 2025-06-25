using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Steamworks;

public class SteamTransport : NetworkTransport
{
    private Dictionary<ulong, CSteamID> clientMap = new();
    private Dictionary<CSteamID, ulong> steamMap = new();

    private CSteamID _targetSteamId; // ← для подключения клиента
    private CSteamID _hostSteamId;

    private ulong localClientId = 0;
    private bool isServer;

    public override ulong ServerClientId => 0;

    public void SetTargetSteamId(CSteamID steamId)
    {
        _targetSteamId = steamId;
    }

    public override void Initialize(NetworkManager networkManager = null)
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("SteamManager is not initialized!");
            return;
        }

        _hostSteamId = SteamUser.GetSteamID();
        Debug.Log($"[SteamTransport] Local Steam ID: {_hostSteamId}");
    }

    public override bool StartServer()
    {
        isServer = true;
        SteamNetworking.AllowP2PPacketRelay(true);
        Debug.Log("[SteamTransport] Server started");
        return true;
    }

    public override bool StartClient()
    {
        isServer = false;
        SteamNetworking.AllowP2PPacketRelay(true);

        if (_targetSteamId == CSteamID.Nil)
        {
            Debug.LogError("Target SteamID is not set! Call SetTargetSteamId() before StartClient().");
            return false;
        }

        // Первичная попытка "разогреть" соединение
        Debug.LogError($"Connecting to: {_targetSteamId}");
        var success = SteamNetworking.SendP2PPacket(_targetSteamId, new byte[] { 1 }, 1, EP2PSend.k_EP2PSendUnreliable, 0);
        Debug.LogError($"Initial ping packet sent: {success}");

        Debug.Log($"[SteamTransport] Connecting to host: {_targetSteamId}");
        return true;
    }

    public override void Send(ulong clientId, ArraySegment<byte> payload, NetworkDelivery networkDelivery)
    {
        if (!clientMap.TryGetValue(clientId, out var steamId))
        {
            Debug.LogWarning($"[SteamTransport] Unknown clientId: {clientId}");
            return;
        }

        var sendType = ConvertDelivery(networkDelivery);
        SteamNetworking.SendP2PPacket(steamId, payload.Array, (uint)payload.Count, sendType, 0);
    }

    private EP2PSend ConvertDelivery(NetworkDelivery delivery) => delivery switch
    {
        NetworkDelivery.Reliable => EP2PSend.k_EP2PSendReliable,
        NetworkDelivery.Unreliable => EP2PSend.k_EP2PSendUnreliable,
        NetworkDelivery.UnreliableSequenced => EP2PSend.k_EP2PSendUnreliableNoDelay,
        _ => EP2PSend.k_EP2PSendReliable
    };

    public override NetworkEvent PollEvent(out ulong clientId, out ArraySegment<byte> payload, out float receiveTime)
    {
        payload = default;
        receiveTime = Time.realtimeSinceStartup;
        clientId = 0;

        while (SteamNetworking.IsP2PPacketAvailable(out uint size, 0))
        {
            Debug.LogError($"Incoming packet detected, size: {size}");
            byte[] buffer = new byte[size];
            if (SteamNetworking.ReadP2PPacket(buffer, size, out uint bytesRead, out CSteamID sender, 0))
            {
                Debug.LogError($"Got packet from {sender} ({bytesRead} bytes)");
                if (!steamMap.ContainsKey(sender))
                {
                    ulong newClientId = (ulong)steamMap.Count + 1;
                    steamMap[sender] = newClientId;
                    clientMap[newClientId] = sender;

                    clientId = newClientId;
                    Debug.Log($"[SteamTransport] New client connected: {sender}");
                    return NetworkEvent.Connect;
                }

                clientId = steamMap[sender];
                payload = new ArraySegment<byte>(buffer, 0, (int)bytesRead);
                return NetworkEvent.Data;
            }
        }

        return NetworkEvent.Nothing;
    }

    public override void DisconnectRemoteClient(ulong clientId)
    {
        if (clientMap.TryGetValue(clientId, out var steamId))
        {
            SteamNetworking.CloseP2PSessionWithUser(steamId);
            clientMap.Remove(clientId);
            steamMap.Remove(steamId);
            Debug.Log($"[SteamTransport] Disconnected remote client {steamId}");
        }
    }

    public override void DisconnectLocalClient()
    {
        foreach (var steamId in steamMap.Keys)
        {
            SteamNetworking.CloseP2PSessionWithUser(steamId);
        }

        clientMap.Clear();
        steamMap.Clear();
        Debug.Log("[SteamTransport] Disconnected all local sessions");
    }

    public override ulong GetCurrentRtt(ulong clientId)
    {
        return 0; // Steam P2P doesn't expose RTT
    }

    public override void Shutdown()
    {
        DisconnectLocalClient();
        Debug.Log("[SteamTransport] Transport shutdown");
    }

    protected override void OnEarlyUpdate()
    {
        if (SteamManager.Initialized)
            SteamAPI.RunCallbacks();
    }

    protected override NetworkTopologyTypes OnCurrentTopology() => NetworkTopologyTypes.ClientServer;
}