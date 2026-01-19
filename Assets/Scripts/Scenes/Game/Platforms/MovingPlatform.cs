using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Raycast
{
    [SerializeField] private LayerMask passengerLayerMask;

    [SerializeField] private Vector3[] localWaypoints;

    [SerializeField] private float speed, waitTime;

    [SerializeField, Range(0f, 1f)] private float easeAmount;

    [SerializeField] private bool cyclic;

    private Vector3[] globalWaypoints;

    private Dictionary<Transform, CharacterController2D> passengerDictionary;

    private List<PassengerMovement> passengerMovementList;

    private int fromWaypointIndex;

    private float percentBetweenWaypoints, nextMoveTime;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        int length = localWaypoints.Length;

        this.globalWaypoints = new Vector3[length];

        for (int i = 0; i < length; i++)
        {
            this.globalWaypoints[i] = this.localWaypoints[i] + base.transform.position;
        }

        this.passengerDictionary = new Dictionary<Transform, CharacterController2D>();
    }

    private void Update()
    {
        this.UpdateRaycastOrigins();

        Vector3 velocity = this.CalculatePlatformMovement();

        this.CalculatePassengerMovement(velocity);

        this.MovePassengers(true);

        base.transform.Translate(velocity);

        this.MovePassengers(false);
    }

    private void OnDrawGizmos()
    {
        if (this.localWaypoints != null)
        {
            Gizmos.color = Color.red;

            float size = 0.3f;

            for (int i = 0; i < this.localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPosition = Application.isPlaying ? this.globalWaypoints[i] : this.localWaypoints[i] + base.transform.position;

                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);
            }
        }
    }

    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < this.nextMoveTime)
        {
            return Vector3.zero;
        }

        this.fromWaypointIndex %= this.globalWaypoints.Length;

        int toWaypointIndex = (this.fromWaypointIndex + 1) % this.globalWaypoints.Length;

        float distanceBetweenWaypoints = Vector3.Distance(this.globalWaypoints[this.fromWaypointIndex], this.globalWaypoints[toWaypointIndex]);

        this.percentBetweenWaypoints += Time.deltaTime * this.speed / distanceBetweenWaypoints;

        this.percentBetweenWaypoints = Mathf.Clamp01(this.percentBetweenWaypoints);

        float easedPercentBetweenWaypoints = this.Ease(this.percentBetweenWaypoints);

        Vector3 newPosition = Vector3.Lerp(this.globalWaypoints[this.fromWaypointIndex], this.globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (this.percentBetweenWaypoints >= 1f)
        {
            this.percentBetweenWaypoints = 0f;

            this.fromWaypointIndex++;

            if (!this.cyclic)
            {
                if (this.fromWaypointIndex >= this.globalWaypoints.Length - 1f)
                {
                    this.fromWaypointIndex = 0;

                    Array.Reverse(this.globalWaypoints);
                }
            }

            this.nextMoveTime = Time.time + this.waitTime;
        }

        Vector3 velocity = newPosition - base.transform.position;

        return velocity;
    }

    private float Ease(float x)
    {
        float a = this.easeAmount + 1f;

        float result = Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1f - x, a));

        return result;
    }

    private void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passengerMovement in this.passengerMovementList)
        {
            CharacterController2D characterController2D;

            if (!this.passengerDictionary.ContainsKey(passengerMovement.Transform))
            {
                characterController2D = passengerMovement.Transform.GetComponent<CharacterController2D>();

                this.passengerDictionary.Add(passengerMovement.Transform, characterController2D);
            }
            else
            {
                characterController2D = this.passengerDictionary[passengerMovement.Transform];
            }

            if (passengerMovement.MoveBeforePlatform == beforeMovePlatform)
            {
                characterController2D.Move(passengerMovement.Velocity, passengerMovement.StandingOnPlatform);
            }
        }
    }

    private void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassangers = new HashSet<Transform>();

        this.passengerMovementList = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (velocity.y != 0f)
        {
            float rayLength = Mathf.Abs(velocity.y) + MovingPlatform.WIDTH;

            for (int i = 0; i < base.VerticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1f) ? base.Origins.BottomLeft : base.Origins.TopLeft;

                rayOrigin += Vector2.right * (base.VerticalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, this.passengerLayerMask);

                if (hit && hit.distance != 0f)
                {
                    if (!movedPassangers.Contains(hit.transform))
                    {
                        movedPassangers.Add(hit.transform);

                        float pushX = (directionY == 1f) ? velocity.x : 0f;
                        float pushY = velocity.y - (hit.distance - MovingPlatform.WIDTH) * directionY;

                        Vector3 push = new Vector3(pushX, pushY);

                        PassengerMovement passengerMovement = new PassengerMovement(hit.transform, push, directionY == 1f, true);

                        this.passengerMovementList.Add(passengerMovement);
                    }
                }
            }
        }

        if (velocity.x != 0f)
        {
            float rayLength = Mathf.Abs(velocity.x) + MovingPlatform.WIDTH;

            for (int i = 0; i < base.HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1f) ? base.Origins.BottomLeft : base.Origins.BottomRight;

                rayOrigin += Vector2.up * (base.HorizontalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, this.passengerLayerMask);

                if (hit && hit.distance != 0f)
                {
                    if (!movedPassangers.Contains(hit.transform))
                    {
                        movedPassangers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - MovingPlatform.WIDTH) * directionX;
                        float pushY = -MovingPlatform.WIDTH;

                        Vector3 push = new Vector3(pushX, pushY);

                        PassengerMovement passengerMovement = new PassengerMovement(hit.transform, push, false, true);

                        this.passengerMovementList.Add(passengerMovement);
                    }
                }
            }
        }

        if (directionY == -1f || velocity.y == 0f && velocity.x != 0f)
        {
            float rayLength = MovingPlatform.WIDTH * 2f;

            for (int i = 0; i < base.VerticalRayCount; i++)
            {
                Vector2 rayOrigin = base.Origins.TopLeft;

                rayOrigin += Vector2.right * base.VerticalRaySpacing * i;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, this.passengerLayerMask);

                if (hit && hit.distance != 0f)
                {
                    if (!movedPassangers.Contains(hit.transform))
                    {
                        movedPassangers.Add(hit.transform);

                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        Vector3 push = new Vector3(pushX, pushY);

                        PassengerMovement passengerMovement = new PassengerMovement(hit.transform, push, true, false);

                        this.passengerMovementList.Add(passengerMovement);
                    }
                }
            }
        }
    }

    private struct PassengerMovement
    {
        public Transform Transform { get; private set; }
        public Vector3 Velocity { get; private set; }
        public bool StandingOnPlatform { get; private set; }
        public bool MoveBeforePlatform { get; private set; }

        public PassengerMovement(Transform transform, Vector3 velocity, bool standingOnPlatform, bool moveBeforePlatform)
        {
            this.Transform = transform;
            this.Velocity = velocity;
            this.StandingOnPlatform = standingOnPlatform;
            this.MoveBeforePlatform = moveBeforePlatform;
        }
    }
}