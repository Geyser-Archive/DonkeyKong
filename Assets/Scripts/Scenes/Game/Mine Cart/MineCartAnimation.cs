using UnityEngine;

[RequireComponent(typeof(MineCart), typeof(Animator))]
public class MineCartAnimation : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject mineCartEffect;

    private MineCart mineCart;

    private Animator animator;

    private float lastSpeed = 0f;

    private float groundedTimer = 0f;

    private float x = 0f;
    private float y = 0f;

    private bool grounded = false;

    private void Awake()
    {
        this.mineCart = base.GetComponent<MineCart>();
        this.animator = base.GetComponent<Animator>();
    }

    private void Start()
    {
        float currentX = base.transform.position.x;
        float currentY = base.transform.position.y;

        this.x = currentX;
        this.y = currentY;
    }

    private void FixedUpdate()
    {
        float speed = this.mineCart.Speed;

        if (speed != this.lastSpeed)
        {
            if (speed > 0f)
            {
                this.sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (speed < 0f)
            {
                this.sprite.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }

            this.lastSpeed = speed;
        }

        float absSpeed = Mathf.Abs(speed);

        this.animator.SetFloat("Speed", absSpeed);

        if (this.mineCart.MineCartController2D.Grounded())
        {
            this.groundedTimer = 0f;
        }
        else
        {
            this.groundedTimer += Time.fixedDeltaTime;
        }

        float absVerticalSpeed = Mathf.Abs(this.mineCart.VerticalSpeed);

        if (absVerticalSpeed > 1f)
        {
            this.animator.SetBool("Grounded", false);

            this.grounded = false;
        }
        else
        {
            if (this.groundedTimer < 0.1f)
            {
                this.animator.SetBool("Grounded", true);

                if (!this.grounded)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

                    Object.Instantiate(this.mineCartEffect, base.transform.position, rotation);
                }

                this.grounded = true;
            }
            else
            {
                this.animator.SetBool("Grounded", false);
            }
        }

        float currentX = base.transform.position.x;
        float currentY = base.transform.position.y;

        float differenceX = currentX - this.x;
        float differenceY = currentY - this.y;

        this.x = currentX;
        this.y = currentY;

        float currentAngle = this.sprite.transform.localRotation.eulerAngles.z;

        float targetAngle = Mathf.Atan2(differenceY, differenceX) * Mathf.Rad2Deg;

        float angle = Mathf.LerpAngle(currentAngle, targetAngle, absSpeed * Time.fixedDeltaTime);

        if (!this.grounded)
        {
            if (currentAngle > 180f)
            {
                currentAngle -= 360f;
            }

            if (targetAngle < currentAngle)
            {
                angle = currentAngle;
            }
        }

        this.sprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}