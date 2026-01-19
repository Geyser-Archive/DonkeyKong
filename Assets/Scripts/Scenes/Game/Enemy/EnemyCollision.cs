using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private GameObject hit;
    [SerializeField] private float height = 2f;

    private bool alternativeDeath = false;

    private bool dead = false;

    public void ActivateAlternativeDeath()
    {
        this.alternativeDeath = true;
    }

    public void AlternativeDeath()
    {
        this.Death(false);
    }

    public void Trigger()
    {
        // Die through DeathZone

        this.Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            float absSpeed = Mathf.Abs(player.Speed);

            bool mounting = player.Mounting;

            if (mounting)
            {
                absSpeed = 0f;
            }

            if (player.VerticalSpeed < 0f)
            {
                player.Bounce(this.height);

                if (other.TryGetComponent<PlayerAnimation>(out var playerAnimation))
                {
                    playerAnimation.Roll();
                }

                this.Die();
            }
            else if (absSpeed >= 10f)
            {
                this.Die();
            }
            else
            {
                if (GameController.Instance != null)
                {
                    player.BounceMin();

                    if (GameController.Instance != null)
                    {
                        GameController.Instance.Die();
                    }
                }
                else
                {
                    other.transform.position = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }

    private void Die()
    {
        if (this.alternativeDeath)
        {
            if (base.TryGetComponent<EnemyNetworkMaster>(out var enemyNetworkMaster))
            {
                enemyNetworkMaster.Die();
            }

            return;
        }

        this.Death();
    }

    private void Death(bool destroy = true)
    {
        if (this.dead)
        {
            return;
        }

        if (this.hit != null)
        {
            Object.Instantiate(this.hit, base.transform.position, Quaternion.identity);
        }

        if (base.TryGetComponent<EnemyMaster>(out var enemyMaster))
        {
            enemyMaster.Death(destroy);
        }
        else
        {
            Object.Destroy(base.gameObject);
        }

        this.dead = true;
    }
}