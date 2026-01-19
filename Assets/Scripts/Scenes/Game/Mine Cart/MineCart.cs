using UnityEngine;

[RequireComponent(typeof(MineCartController2D))]
public class MineCart : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minJumpHeight = 1f;
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float timeToJumpApex = 0.4f;
    [SerializeField] private float accelerationTime = 0.05f;

    public MineCartController2D MineCartController2D => this.mineCartController2D;
    private MineCartController2D mineCartController2D;

    public float Speed => this.velocity.x;
    public float VerticalSpeed => this.velocity.y;
    private Vector3 velocity;

    private Vector2 directionalInput;

    private float gravity;

    private float minJumpVelocity, maxJumpVelocity;

    private float velocityXSmoothing;

    public void DirectionalInput(Vector2 input)
    {
        this.directionalInput = input;
    }

    public void JumpInputDown()
    {
        this.velocity.y = this.maxJumpVelocity;
    }

    public void JumpInputUp()
    {
        if (this.velocity.y > this.minJumpVelocity)
        {
            this.velocity.y = this.minJumpVelocity;
        }
    }

    public void Bounce(float velocity)
    {
        this.velocity.y = velocity;
    }

    private void Awake()
    {
        this.mineCartController2D = base.GetComponent<MineCartController2D>();
    }

    private void Start()
    {
        this.gravity = -(2f * this.maxJumpHeight) / Mathf.Pow(this.timeToJumpApex, 2);

        this.minJumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(this.gravity) * this.minJumpHeight);
        this.maxJumpVelocity = Mathf.Abs(this.gravity) * this.timeToJumpApex;

    }

    private void Update()
    {
        this.CalculateVelocity();

        this.mineCartController2D.Move(this.velocity * Time.deltaTime, this.directionalInput);

        if (this.mineCartController2D.AboveOrBelow())
        {
            if (this.mineCartController2D.SlidingDownMaxSlope())
            {
                this.velocity.y += this.mineCartController2D.SlopeNormal().y * -this.gravity * Time.deltaTime;
            }
            else
            {
                this.velocity.y = 0f;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = this.directionalInput.x * this.speed;

        this.velocity.x = Mathf.SmoothDamp(this.velocity.x, targetVelocityX, ref this.velocityXSmoothing, this.accelerationTime);
        this.velocity.y += this.gravity * Time.deltaTime;
    }
}