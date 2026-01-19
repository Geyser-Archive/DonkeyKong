using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void Collect();

    private bool alternative = false;

    private bool collected = false;

    public void ActivateAlternative()
    {
        this.alternative = true;
    }

    public void AlternativeCollect()
    {
        if (this.collected)
        {
            return;
        }

        this.collected = true;

        this.Collect();
    }

    public void DestroyAfterCollected()
    {
        if (!this.alternative)
        {
            Object.Destroy(base.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.collected)
            {
                return;
            }

            if (this.alternative)
            {
                if (base.TryGetComponent<CollectableNetworkMaster>(out CollectableNetworkMaster collectableNetworkMaster))
                {
                    collectableNetworkMaster.Collect();
                }

                return;
            }

            this.collected = true;

            this.Collect();
        }
    }
}