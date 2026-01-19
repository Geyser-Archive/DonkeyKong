using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CannonBall : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private float height = 5f;
    [SerializeField] private float deathDelay = 2f;

    [SerializeField] private LayerMask collisionMask;

    private Rigidbody2D rb;

    private bool dead = false;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.dead)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.Die();
            }
        }

        if ((this.collisionMask.value & 1 << other.gameObject.layer) > 0)
        {
            this.Die();
        }
    }

    private void Die()
    {
        this.dead = true;

        this.rb.velocity = new Vector2(0f, this.height);

        base.Invoke(nameof(this.Death), this.deathDelay);

        if (this.prefab != null)
        {
            Object.Instantiate(this.prefab, base.transform.position, Quaternion.identity);
        }
    }

    private void Death()
    {
        Object.Destroy(this.gameObject);
    }
}