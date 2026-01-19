using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : NetworkBehaviour
{
    public static NetworkController Instance => NetworkController.instance;
    private static NetworkController instance;

    [SerializeField] private GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += this.OnLoadEventCompleted;
        }
    }

    public void Stop()
    {
        NetworkManager.Singleton.Shutdown();
    }

    private void Awake()
    {
        NetworkController.instance = this;
    }

    private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (var player in NetworkManager.Singleton.ConnectedClientsList)
        {
            this.SpawnPlayer(player.ClientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject player = Object.Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);

        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}