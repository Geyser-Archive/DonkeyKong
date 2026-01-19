using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingPlatform : Raycast
{
    [SerializeField] private LayerMask passengerLayerMask;

    [SerializeField] private Vector3 move;

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        this.UpdateRaycastOrigins();

        Vector3 velocity = this.move * Time.deltaTime;

        this.MovePassengers(velocity);

        base.transform.Translate(velocity);
    }

    private void MovePassengers(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (velocity.y != 0f)
        {
            float rayLength = Mathf.Abs(velocity.y) + SimpleMovingPlatform.WIDTH;

            for (int i = 0; i < base.VerticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1f) ? base.Origins.BottomLeft : base.Origins.TopLeft;

                rayOrigin += Vector2.right * (base.VerticalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, this.passengerLayerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1f) ? velocity.x : 0f;
                        float pushY = velocity.y - (hit.distance - SimpleMovingPlatform.WIDTH) * directionY;

                        Vector3 push = new Vector3(pushX, pushY);

                        hit.transform.Translate(push);
                    }
                }
            }
        }

        if (velocity.x != 0f)
        {
            float rayLength = Mathf.Abs(velocity.x) + SimpleMovingPlatform.WIDTH;

            for (int i = 0; i < base.HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1f) ? base.Origins.BottomLeft : base.Origins.BottomRight;

                rayOrigin += Vector2.up * (base.HorizontalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, this.passengerLayerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - SimpleMovingPlatform.WIDTH) * directionX;
                        float pushY = 0f;

                        Vector3 push = new Vector3(pushX, pushY);

                        hit.transform.Translate(push);
                    }
                }
            }
        }

        if (directionY == -1f || velocity.y == 0f && velocity.x != 0f)
        {
            float rayLength = SimpleMovingPlatform.WIDTH * 2f;

            for (int i = 0; i < base.VerticalRayCount; i++)
            {
                Vector2 rayOrigin = base.Origins.TopLeft;

                rayOrigin += Vector2.right * base.VerticalRaySpacing * i;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, this.passengerLayerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        Vector3 push = new Vector3(pushX, pushY);

                        hit.transform.Translate(push);
                    }
                }
            }
        }
    }
}