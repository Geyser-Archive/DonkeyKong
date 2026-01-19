using Unity.Netcode;

public class HitNetworkMaster : NetworkBehaviour
{
    public void Destroy()
    {
        if (base.IsOwner)
        {
            NetworkObject.Destroy(base.gameObject);
        }
    }
}