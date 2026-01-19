using UnityEngine;

[RequireComponent(typeof(EnemyController2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float accelerationTime = 0f;
    [SerializeField] private float gravity = -50f;

    public EnemyController2D EnemyController2D => this.enemyController2D;
    private EnemyController2D enemyController2D;

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
        this.enemyController2D = base.GetComponent<EnemyController2D>();
    }

    private void Update()
    {
        this.CalculateVelocity();

        this.enemyController2D.Move(this.velocity * Time.deltaTime);

        if (this.enemyController2D.AboveOrBelow())
        {
            if (this.enemyController2D.SlidingDownMaxSlope())
            {
                this.velocity.y += this.enemyController2D.SlopeNormal().y * -this.gravity * Time.deltaTime;
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