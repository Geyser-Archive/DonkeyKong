using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f, runSpeed = 5f, rollSpeed = 10f;
    [SerializeField] private float minJumpHeight = 1f;
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float timeToJumpApex = 0.4f;

    [SerializeField] private float accelerationTimeAirborne = 0.2f;
    [SerializeField] private float accelerationTimeGrounded = 0.1f;

    [SerializeField] private Vector2 wallJumpClimb = new Vector2(7.5f, 16f);
    [SerializeField] private Vector2 wallJumpOff = new Vector2(8.5f, 7f);
    [SerializeField] private Vector2 wallLeap = new Vector2(18f, 17f);

    [SerializeField] private float wallSlideSpeedMax = 3f;
    [SerializeField] private float wallStickTime = 0.1f;

    public CharacterController2D CharacterController2D => this.characterController2D;
    private CharacterController2D characterController2D;

    public bool Mounting => this.mount != null;
    private Transform mount;

    public float Speed => this.velocity.x;
    public float VerticalSpeed => this.velocity.y;
    private Vector3 velocity;

    private Vector2 directionalInput;

    private float gravity;

    private float minJumpVelocity, maxJumpVelocity;
    private float timeToWallUnstick;

    private float velocityXSmoothing;

    private int wallDirX;

    private bool sprinting;

    private bool wallSliding;

    public bool Mount(Transform transform)
    {
        if (this.mount != null)
        {
            return false;
        }

        this.mount = transform;

        this.velocity = Vector3.zero;

        return true;
    }

    public void Dismount()
    {
        this.mount = null;
    }

    public void DirectionalInput(Vector2 input)
    {
        this.directionalInput = input;
    }

    public void SprintInputDown()
    {
        this.sprinting = true;
    }

    public void SprintInputUp()
    {
        this.sprinting = false;
    }

    public void Roll()
    {
        if (this.characterController2D.Grounded())
        {
            base.StartCoroutine(this.Rolling());
        }
    }

    public void JumpInputDown()
    {
        if (this.wallSliding)
        {
            if (this.wallDirX == this.directionalInput.x)
            {
                this.velocity.x = -this.wallDirX * this.wallJumpClimb.x;
                this.velocity.y = this.wallJumpClimb.y;
            }
            else if (this.directionalInput.x == 0f)
            {
                this.velocity.x = -this.wallDirX * this.wallJumpOff.x;
                this.velocity.y = this.wallJumpOff.y;
            }
            else
            {
                this.velocity.x = -this.wallDirX * this.wallLeap.x;
                this.velocity.y = this.wallLeap.y;
            }
        }

        if (this.characterController2D.Grounded())
        {
            if (this.characterController2D.SlidingDownMaxSlope())
            {
                if (this.directionalInput.x != -Mathf.Sign(this.characterController2D.SlopeNormal().x))
                {
                    this.velocity.y = this.maxJumpVelocity * this.characterController2D.SlopeNormal().y;
                    this.velocity.x = this.maxJumpVelocity * this.characterController2D.SlopeNormal().x;
                }
            }
            else
            {
                this.velocity.y = this.maxJumpVelocity;
            }
        }
    }

    public void JumpInputUp()
    {
        if (this.velocity.y > this.minJumpVelocity)
        {
            this.velocity.y = this.minJumpVelocity;
        }
    }

    public void BounceMin(float multiplier = 1f)
    {
        this.Bounce(this.minJumpVelocity * multiplier);
    }

    public void BounceMax(float multiplier = 1f)
    {
        this.Bounce(this.maxJumpVelocity * multiplier);
    }

    public void Bounce(float height, float multiplier = 1f)
    {
        float velocity = Mathf.Sqrt(2f * Mathf.Abs(this.gravity) * height);

        this.Bounce(velocity * multiplier);
    }

    public void Launch(Vector2 direction, float strength, float multiplier = 1f)
    {
        float velocity = Mathf.Sqrt(2f * Mathf.Abs(this.gravity) * strength);

        this.velocity.x = direction.x * velocity * multiplier;
        this.velocity.y = direction.y * velocity * multiplier;
    }

    private void Awake()
    {
        this.characterController2D = base.GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        this.gravity = -(2f * this.maxJumpHeight) / Mathf.Pow(this.timeToJumpApex, 2);

        this.minJumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(this.gravity) * this.minJumpHeight);
        this.maxJumpVelocity = Mathf.Abs(this.gravity) * this.timeToJumpApex;
    }

    private void Update()
    {
        if (this.mount != null)
        {
            this.transform.position = this.mount.position;

            if (this.mount.TryGetComponent<MineCart>(out var mineCart))
            {
                float velocityX = mineCart.Speed;
                float velocityY = mineCart.VerticalSpeed;

                this.velocity = new Vector3(velocityX, velocityY, 0f);
            }

            if (this.mount.TryGetComponent<MineCartController2D>(out var mineCartController2D))
            {
                Vector2 zero = Vector2.zero;

                Vector2 directionalInput = mineCartController2D.Input;

                this.characterController2D.Move(zero, directionalInput);
            }

            return;
        }

        this.CalculateVelocity();

        this.WallSliding();

        this.characterController2D.Move(this.velocity * Time.deltaTime, this.directionalInput);

        if (this.characterController2D.AboveOrBelow())
        {
            if (this.characterController2D.SlidingDownMaxSlope())
            {
                this.velocity.y += this.characterController2D.SlopeNormal().y * -this.gravity * Time.deltaTime;
            }
            else
            {
                this.velocity.y = 0f;
            }
        }
    }

    private void Bounce(float velocity)
    {
        if (this.mount == null)
        {
            this.velocity.y = velocity;
        }
        else
        {
            if (this.mount.TryGetComponent<MineCart>(out var mineCart))
            {
                mineCart.Bounce(velocity);
            }
        }
    }

    private void CalculateVelocity()
    {
        float speed = this.sprinting ? this.runSpeed : this.walkSpeed;

        float targetVelocityX = this.directionalInput.x * speed;

        this.velocity.x = Mathf.SmoothDamp(this.velocity.x, targetVelocityX, ref this.velocityXSmoothing, this.characterController2D.Grounded() ? this.accelerationTimeGrounded : this.accelerationTimeAirborne);
        this.velocity.y += this.gravity * Time.deltaTime;
    }

    private void WallSliding()
    {
        this.wallDirX = this.characterController2D.LeftOrRight() ? this.characterController2D.FaceDirection() : 1;

        this.wallSliding = false;

        if (this.characterController2D.LeftOrRight() && !this.characterController2D.Grounded() && this.velocity.y < 0f)
        {
            bool value = true;

            if (value)
            {
                return;
            }

            this.wallSliding = true;

            if (this.velocity.y < -this.wallSlideSpeedMax)
            {
                this.velocity.y = -this.wallSlideSpeedMax;
            }

            if (this.timeToWallUnstick > 0f)
            {
                this.velocityXSmoothing = 0f;
                this.velocity.x = 0f;

                if (this.directionalInput.x != this.wallDirX && this.directionalInput.x != 0f)
                {
                    this.timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    this.timeToWallUnstick = this.wallStickTime;
                }
            }
            else
            {
                this.timeToWallUnstick = this.wallStickTime;
            }
        }
    }

    private IEnumerator Rolling()
    {
        float time = 0f;

        while (time < 0.5f)
        {
            time += Time.deltaTime;

            this.velocity.x = this.characterController2D.FaceDirection() * this.rollSpeed;

            yield return null;
        }

        this.velocity.x = this.characterController2D.FaceDirection();
    }
}