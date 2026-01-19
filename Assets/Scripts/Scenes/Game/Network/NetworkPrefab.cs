using Unity.Netcode;
using UnityEngine;

public class NetworkPrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Awake()
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            GameObject gameObject = Object.Instantiate(this.prefab, base.transform.position, base.transform.rotation, base.transform.parent);

            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();

            networkObject.Spawn();

            Object.Destroy(base.gameObject);
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            Object.Destroy(base.gameObject);
        }
    }
}