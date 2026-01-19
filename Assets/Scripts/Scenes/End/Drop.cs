using UnityEngine;

public class Drop : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;

    private bool active = false;

    private void Awake()
    {
        this.myRigidbody2D = base.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        base.Invoke(nameof(this.Activate), 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            if (this.active)
            {
                this.myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void Activate()
    {
        this.active = true;
    }
}