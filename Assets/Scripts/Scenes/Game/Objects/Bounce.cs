using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float height = 4f;

    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.Bounce(this.height);

            if (other.TryGetComponent<PlayerAnimation>(out var playerAnimation))
            {
                playerAnimation.Roll();
            }
        }

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.Bounce(this.height);
        }

        if (this.animator != null)
        {
            this.animator.SetTrigger("Bounce");
        }
    }
}