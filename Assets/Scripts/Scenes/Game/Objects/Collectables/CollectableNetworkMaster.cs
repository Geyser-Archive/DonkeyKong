using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collectable))]
public class CollectableNetworkMaster : NetworkBehaviour
{
    private Collectable collectable;

    public void Collect()
    {
        this.HandleCollectServerRpc();
    }

    private void Awake()
    {
        this.collectable = base.GetComponent<Collectable>();
    }

    private void Start()
    {
        this.collectable.ActivateAlternative();
    }

    private void DestroyAfterCollected()
    {
        NetworkObject.Destroy(base.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleCollectServerRpc()
    {
        this.HandleCollectClientRpc();

        base.Invoke(nameof(this.DestroyAfterCollected), 1f);
    }

    [ClientRpc]
    private void HandleCollectClientRpc()
    {
        this.collectable.AlternativeCollect();
    }
}