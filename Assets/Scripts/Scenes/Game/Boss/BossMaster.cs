using UnityEngine;

[RequireComponent(typeof(BossAnimation), typeof(BoxCollider2D))]
public class BossMaster : MonoBehaviour
{
    [SerializeField] private GameObject gateway;

    private BossAnimation bossAnimation;

    private BoxCollider2D boxCollider2D;

    public void Death()
    {
        if (this.gateway != null)
        {
            this.gateway.SetActive(true);
        }

        this.bossAnimation.Death();

        base.Invoke(nameof(this.DestroyAfterDeath), 60f);
    }

    public void DisableCollider()
    {
        this.boxCollider2D.enabled = false;
    }

    private void Awake()
    {
        this.bossAnimation = base.GetComponent<BossAnimation>();
        this.boxCollider2D = base.GetComponent<BoxCollider2D>();
    }

    private void DestroyAfterDeath()
    {
        Object.Destroy(base.gameObject);
    }
}