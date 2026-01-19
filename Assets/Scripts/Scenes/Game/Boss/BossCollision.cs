using UnityEngine;

[RequireComponent(typeof(BossHealth))]
public class BossCollision : MonoBehaviour
{
    [SerializeField] private GameObject hit;
    [SerializeField] private float height = 2f, strength = 4f;

    public BossHealth BossHealth => this.bossHealth;
    private BossHealth bossHealth;

    public bool Vulnerable { get => this.vulnerable; set => this.vulnerable = value; }
    private bool vulnerable = true;

    private bool dead = false;

    public void Trigger()
    {
        // Die through DeathZone

        this.Death();
    }

    private void Awake()
    {
        this.bossHealth = base.GetComponent<BossHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.dead)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            Transform playerTransform = player.transform;

            int direction = playerTransform.position.x > 0f ? -1 : 1;

            /*
            
            float speed = player.Speed;

            int direction = speed < 0f ? 1 : -1;
            
            */

            Vector2 force = new Vector2(direction, 0f);

            if (player.VerticalSpeed < 0f && this.vulnerable)
            {
                player.Launch(force, this.strength);

                player.Bounce(this.height);

                if (other.TryGetComponent<PlayerAnimation>(out var playerAnimation))
                {
                    playerAnimation.Roll();
                }

                if (this.hit != null)
                {
                    Object.Instantiate(this.hit, base.transform.position, Quaternion.identity);
                }

                this.bossHealth.Damage(1);
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

            if (this.bossHealth.IsDead())
            {
                this.Death();
            }
        }
    }

    private void Death()
    {
        if (this.dead)
        {
            return;
        }

        if (base.TryGetComponent<BossMaster>(out var bossMaster))
        {
            bossMaster.Death();
        }
        else
        {
            Object.Destroy(base.gameObject);
        }

        this.dead = true;
    }
}