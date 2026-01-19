using UnityEngine;

public class Hit : MonoBehaviour
{
    private void Start()
    {
        base.Invoke(nameof(Destroy), 1f);
    }

    private void Destroy()
    {
        if (base.TryGetComponent(out HitNetworkMaster hitNetworkMaster))
        {
            hitNetworkMaster.Destroy();
        }
        else
        {
            Object.Destroy(base.gameObject);
        }
    }
}