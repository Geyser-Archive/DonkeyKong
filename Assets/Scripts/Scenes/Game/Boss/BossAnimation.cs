using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class BossAnimation : MonoBehaviour
{

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public void Flip()
    {
        this.spriteRenderer.flipX = !this.spriteRenderer.flipX;
    }

    public void Death()
    {
        this.animator.SetTrigger("Death");
    }

    private void Awake()
    {
        this.animator = base.GetComponent<Animator>();
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
    }
}