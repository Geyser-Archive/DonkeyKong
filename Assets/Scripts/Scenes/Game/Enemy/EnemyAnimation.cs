using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(Animator), typeof(SpriteRenderer))]
public class EnemyAnimation : MonoBehaviour
{
    private Enemy enemy;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public void Death()
    {
        this.animator.SetTrigger("Death");
    }

    private void Awake()
    {
        this.enemy = base.GetComponent<Enemy>();
        this.animator = base.GetComponent<Animator>();
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (this.enemy.Speed > 0f)
        {
            this.spriteRenderer.flipX = false;
        }
        else if (this.enemy.Speed < 0f)
        {
            this.spriteRenderer.flipX = true;
        }
    }
}