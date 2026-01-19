using UnityEngine;

[RequireComponent(typeof(EnemyAnimation), typeof(BoxCollider2D))]
public class EnemyMaster : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;

    private BoxCollider2D boxCollider2D;

    public void Death(bool destroy = true)
    {
        this.enemyAnimation.Death();

        Enemy enemy = base.GetComponent<Enemy>();

        enemy.EnemyController2D.RemoveMask();

        if (destroy)
        {
            base.Invoke(nameof(this.DestroyAfterDeath), 1f);
        }

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Kill();
        }
    }

    public void DisableCollider()
    {
        this.boxCollider2D.enabled = false;
    }

    private void Awake()
    {
        this.enemyAnimation = base.GetComponent<EnemyAnimation>();
        this.boxCollider2D = base.GetComponent<BoxCollider2D>();
    }

    private void DestroyAfterDeath()
    {
        Object.Destroy(base.gameObject);
    }
}