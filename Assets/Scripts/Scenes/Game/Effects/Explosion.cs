using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        base.Invoke(nameof(Destroy), 0.5f);
    }

    private void Destroy()
    {
        Object.Destroy(base.gameObject);
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
}