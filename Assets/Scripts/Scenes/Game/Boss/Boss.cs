using UnityEngine;

[RequireComponent(typeof(BossController2D))]
public class Boss : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float accelerationTime = 0f;
    [SerializeField] private float gravity = -50f;

    public BossController2D BossController2D => this.bossController2D;
    private BossController2D bossController2D;

    public float Speed => this.velocity.x;
    public float VerticalSpeed => this.velocity.y;
    private Vector3 velocity;

    private Vector2 directionalInput;

    private float velocityXSmoothing;

    public void DirectionalInput(Vector2 input)
    {
        this.directionalInput = input;
    }

    public void Bounce(float height, float multiplier = 1f)
    {
        float velocity = Mathf.Sqrt(2f * Mathf.Abs(this.gravity) * height);

        this.velocity.y = velocity * multiplier;
    }

    private void Awake()
    {
        this.bossController2D = base.GetComponent<BossController2D>();
    }

    private void Update()
    {
        this.CalculateVelocity();

        this.bossController2D.Move(this.velocity * Time.deltaTime);

        if (this.bossController2D.AboveOrBelow())
        {
            if (this.bossController2D.SlidingDownMaxSlope())
            {
                this.velocity.y += this.bossController2D.SlopeNormal().y * -this.gravity * Time.deltaTime;
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