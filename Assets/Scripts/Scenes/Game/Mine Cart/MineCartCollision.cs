using UnityEngine;

[RequireComponent(typeof(MineCartPassenger))]
public class MineCartCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Callback callback;

    private MineCartPassenger mineCartPassenger;

    public void Trigger()
    {
        this.Die();
    }

    private void Awake()
    {
        this.mineCartPassenger = base.GetComponent<MineCartPassenger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.callback != null) {
                this.callback.Trigger();
            }

            mineCartPassenger.Passenger(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            this.Die();
        }
    }

    private void Die()
    {
        this.mineCartPassenger.Die();

        if (this.explosion != null)
        {
            Object.Instantiate(this.explosion, base.transform.position, Quaternion.identity);
        }

        Object.Destroy(base.gameObject);
    }
}