using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MineCart))]
public class MineCartInput : MonoBehaviour
{
    private MineCart mineCart;

    private bool active = false;

    private float vertical = 0f;

    private float timer = 0f;

    public void Activate()
    {
        this.active = true;
    }

    public void Vertical(float vertical)
    {
        this.vertical = vertical;
    }

    public void JumpDown()
    {
        base.StopAllCoroutines();

        base.StartCoroutine(this.Jump());
    }

    public void JumpUp()
    {
        base.Invoke(nameof(this.StopJump), this.timer);
    }

    private void Awake()
    {
        this.mineCart = base.GetComponent<MineCart>();
    }

    private void Update()
    {
        if (!this.active)
        {
            return;
        }

        Vector2 directionalInput;

        directionalInput = new Vector2(1f, this.vertical);

        this.mineCart.DirectionalInput(directionalInput);
    }

    private void StopJump()
    {
        this.mineCart.JumpInputUp();
    }

    private IEnumerator Jump()
    {
        this.timer = 0f;

        while (this.timer < 0.1f)
        {
            if (this.mineCart.MineCartController2D.Grounded())
            {
                this.mineCart.JumpInputDown();

                yield break;
            }

            this.timer += Time.deltaTime;

            yield return null;
        }
    }
}