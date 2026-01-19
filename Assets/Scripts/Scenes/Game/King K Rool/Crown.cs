using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Crown : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    private Rigidbody2D rb;

    public void Direction(int direction)
    {
        this.rb.velocity = new Vector2(direction * this.speed, 0f);
    }

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        base.Invoke(nameof(this.Death), 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.Die();
            }
        }
    }

    private void Death()
    {
        Object.Destroy(this.gameObject);
    }
}