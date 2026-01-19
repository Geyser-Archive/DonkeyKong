using UnityEngine;

public class MineCartController2D : Raycast
{
    [SerializeField] private LayerMask collisionLayerMask;

    [SerializeField] private float maxSlopeAngle = 90f;

    public Vector2 Input { get; private set; }

    private CollisionInfo collisionInfo;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        this.collisionInfo.FaceDirection = 1;
    }

    public bool AboveOrBelow()
    {
        return this.collisionInfo.Above || this.collisionInfo.Below;
    }

    public bool Grounded()
    {
        return this.collisionInfo.Below;
    }

    public bool SlidingDownMaxSlope()
    {
        return this.collisionInfo.SlidingDownMaxSlope;
    }

    public int FaceDirection()
    {
        return this.collisionInfo.FaceDirection;
    }

    public Vector2 SlopeNormal()
    {
        return this.collisionInfo.SlopeNormal;
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        this.UpdateRaycastOrigins();

        this.collisionInfo.Reset();

        this.collisionInfo.MoveAmountOld = moveAmount;

        this.Input = input;

        if (moveAmount.y < 0f)
        {
            this.DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0f)
        {
            this.collisionInfo.FaceDirection = (int)Mathf.Sign(moveAmount.x);
        }

        this.HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0f)
        {
            this.VerticalCollisions(ref moveAmount);
        }

        base.transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            this.collisionInfo.Below = true;
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = this.collisionInfo.FaceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + MineCartController2D.WIDTH;

        if (Mathf.Abs(moveAmount.x) < MineCartController2D.WIDTH)
        {
            rayLength = 2f * MineCartController2D.WIDTH;
        }

        for (int i = 0; i < base.HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1f) ? base.Origins.BottomLeft : base.Origins.BottomRight;

            rayOrigin += Vector2.up * (base.HorizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, this.collisionLayerMask);

            if (hit)
            {
                if (hit.distance == 0f)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= this.maxSlopeAngle)
                {
                    if (this.collisionInfo.DescendingSlope)
                    {
                        this.collisionInfo.DescendingSlope = false;

                        moveAmount = this.collisionInfo.MoveAmountOld;
                    }

                    float distanceToSlopeStart = 0f;

                    if (slopeAngle != this.collisionInfo.SlopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - MineCartController2D.WIDTH;

                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    this.ClimbSlope(ref moveAmount, slopeAngle, hit.normal);

                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!this.collisionInfo.ClimbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - MineCartController2D.WIDTH) * directionX;

                    rayLength = hit.distance;

                    if (this.collisionInfo.ClimbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(this.collisionInfo.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    this.collisionInfo.Left = directionX == -1f;
                    this.collisionInfo.Right = directionX == 1f;
                }
            }
        }
    }

    private void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + MineCartController2D.WIDTH;

        for (int i = 0; i < base.VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1f) ? base.Origins.BottomLeft : base.Origins.TopLeft;

            rayOrigin += Vector2.right * (base.VerticalRaySpacing * i + moveAmount.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, this.collisionLayerMask);

            if (hit)
            {
                if (directionY == 1f || hit.distance == 0f)
                {
                    continue;
                }


                if (this.collisionInfo.FallingThroughPlatform)
                {
                    continue;
                }

                if (this.Input.y == -1f && hit.collider.CompareTag("Through"))
                {
                    this.collisionInfo.FallingThroughPlatform = true;

                    base.Invoke(nameof(this.ResetFallingThroughPlatform), 0.05f);

                    continue;
                }

                moveAmount.y = (hit.distance - MineCartController2D.WIDTH) * directionY;

                rayLength = hit.distance;

                if (this.collisionInfo.ClimbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(this.collisionInfo.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                this.collisionInfo.Below = directionY == -1f;
                this.collisionInfo.Above = directionY == 1f;
            }
        }

        if (this.collisionInfo.ClimbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);

            rayLength = Mathf.Abs(moveAmount.x) + MineCartController2D.WIDTH;

            Vector2 rayOrigin = ((directionX == -1f) ? Origins.BottomLeft : base.Origins.BottomRight) + Vector2.up * moveAmount.y;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, this.collisionLayerMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != collisionInfo.SlopeAngle)
                {
                    moveAmount.x = (hit.distance - MineCartController2D.WIDTH) * directionX;

                    this.collisionInfo.SlopeAngle = slopeAngle;
                    this.collisionInfo.SlopeNormal = hit.normal;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbVelocityY)
        {
            moveAmount.y = climbVelocityY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

            this.collisionInfo.Below = true;
            this.collisionInfo.ClimbingSlope = true;

            this.collisionInfo.SlopeAngle = slopeAngle;
            this.collisionInfo.SlopeNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 moveAmount)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(base.Origins.BottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + MineCartController2D.WIDTH, this.collisionLayerMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(base.Origins.BottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + MineCartController2D.WIDTH, this.collisionLayerMask);

        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            this.SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            this.SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!this.collisionInfo.SlidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);

            Vector2 rayOrigin = (directionX == -1f) ? base.Origins.BottomRight : base.Origins.BottomLeft;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, this.collisionLayerMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0f && slopeAngle <= this.maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - MineCartController2D.WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendVelocityY;

                            this.collisionInfo.Below = true;
                            this.collisionInfo.DescendingSlope = true;

                            this.collisionInfo.SlopeAngle = slopeAngle;
                            this.collisionInfo.SlopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle > this.maxSlopeAngle)
            {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                this.collisionInfo.SlidingDownMaxSlope = true;

                this.collisionInfo.SlopeAngle = slopeAngle;
                this.collisionInfo.SlopeNormal = hit.normal;
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        this.collisionInfo.FallingThroughPlatform = false;
    }

    private struct CollisionInfo
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Above { get; set; }
        public bool Below { get; set; }

        public bool ClimbingSlope { get; set; }
        public bool DescendingSlope { get; set; }
        public bool SlidingDownMaxSlope { get; set; }

        public bool FallingThroughPlatform { get; set; }

        public float SlopeAngleOld { get; set; }
        public float SlopeAngle { get; set; }

        public int FaceDirection { get; set; }

        public Vector2 MoveAmountOld { get; set; }
        public Vector2 SlopeNormal { get; set; }

        public void Reset()
        {
            this.Left = this.Right = this.Above = this.Below = false;
            this.ClimbingSlope = this.DescendingSlope = this.SlidingDownMaxSlope = false;

            this.SlopeAngleOld = this.SlopeAngle;
            this.SlopeAngle = 0f;

            this.SlopeNormal = Vector2.zero;
        }
    }
}