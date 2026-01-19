using UnityEngine;

[RequireComponent(typeof(Player), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    private Player player;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private float groundedTimer = 0f;

    public void Death()
    {
        this.animator.SetTrigger("Death");
    }

    public void Revive()
    {
        this.animator.SetTrigger("Revive");
    }

    public void Roll()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("DonkeyKong_Roll"))
        {
            return;
        }

        this.animator.SetTrigger("Roll");
    }

    private void Awake()
    {
        this.player = base.GetComponent<Player>();
        this.animator = base.GetComponent<Animator>();
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        bool mounting = this.player.Mounting;

        if (mounting)
        {
            this.spriteRenderer.enabled = false;

            return;
        }

        if (!this.spriteRenderer.enabled)
        {
            this.spriteRenderer.enabled = true;
        }

        float absSpeed = Mathf.Abs(this.player.Speed);

        this.animator.SetFloat("Speed", absSpeed);

        if (this.player.CharacterController2D.Grounded())
        {
            this.groundedTimer = 0f;
        }
        else
        {
            this.groundedTimer += Time.fixedDeltaTime;
        }

        if (this.player.VerticalSpeed > 1f)
        {
            this.animator.SetBool("Grounded", false);
        }
        else
        {
            this.animator.SetBool("Grounded", this.groundedTimer < 0.2f);
        }

        if (this.player.Speed > 0f)
        {
            this.spriteRenderer.flipX = false;
        }
        else if (this.player.Speed < 0f)
        {
            this.spriteRenderer.flipX = true;
        }
    }
}