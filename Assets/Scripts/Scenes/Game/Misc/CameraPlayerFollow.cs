using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPlayerFollow : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D bounds;
    [SerializeField] private Vector2 focusAreaSize = new Vector2(3f, 5f);
    [SerializeField] private float lookAheadDistanceX = 2f;
    [SerializeField] private float lookSmoothTimeX = 0.5f;
    [SerializeField] private float verticalSmoothTime = 0.2f;
    [SerializeField] private float verticalOffset = 0f;

    private Camera myCamera;
    private CharacterController2D characterController2D;
    private FocusArea focusArea;

    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirectionX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    private bool lookAheadStopped;

    private bool stopped;

    private void Awake()
    {
        this.myCamera = base.GetComponent<Camera>();
    }

    private void Start()
    {
        this.FindPlayer();

        this.focusArea = new FocusArea(this.characterController2D.BoxCollider2D.bounds, this.focusAreaSize);

        this.stopped = false;
    }

    private void LateUpdate()
    {
        if (this.stopped)
        {
            return;
        }

        if (this.characterController2D.BoxCollider2D == null)
        {
            return;
        }

        this.focusArea.Refresh(this.characterController2D.BoxCollider2D.bounds);

        Vector2 focusPosition = this.focusArea.Center + Vector2.up * this.verticalOffset;

        if (this.focusArea.Velocity.x != 0)
        {
            this.lookAheadDirectionX = Mathf.Sign(this.focusArea.Velocity.x);

            if (Mathf.Sign(this.characterController2D.Input.x) == Mathf.Sign(this.focusArea.Velocity.x) && this.characterController2D.Input.x != 0)
            {
                this.targetLookAheadX = this.lookAheadDirectionX * this.lookAheadDistanceX;

                this.lookAheadStopped = false;
            }
            else
            {
                if (!this.lookAheadStopped)
                {
                    this.targetLookAheadX = this.currentLookAheadX + (this.lookAheadDirectionX * this.lookAheadDistanceX - this.currentLookAheadX) / 4f;

                    this.lookAheadStopped = true;
                }
            }
        }

        this.currentLookAheadX = Mathf.SmoothDamp(this.currentLookAheadX, this.targetLookAheadX, ref this.smoothLookVelocityX, this.lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(this.transform.position.y, focusPosition.y, ref this.smoothVelocityY, this.verticalSmoothTime);
        focusPosition += Vector2.right * this.currentLookAheadX;

        Vector3 newPosition = (Vector3)focusPosition + Vector3.forward * -10f;

        this.Apply(newPosition);
    }

    private void OnEnable()
    {
        base.StartCoroutine(this.Wait());
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            this.characterController2D = player.GetComponent<CharacterController2D>();
        }
    }

    private void Apply(Vector3 position)
    {
        this.transform.position = this.ClampPosition(position);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        Vector3 difference = position - this.transform.position;

        Vector3 topLeftView = this.myCamera.ViewportToWorldPoint(new Vector3(0f, 1f, -this.transform.position.z));
        Vector3 topRightView = this.myCamera.ViewportToWorldPoint(new Vector3(1f, 1f, -this.transform.position.z));
        Vector3 bottomLeftView = this.myCamera.ViewportToWorldPoint(new Vector3(0f, 0f, -this.transform.position.z));
        Vector3 bottomRightView = this.myCamera.ViewportToWorldPoint(new Vector3(1f, 0f, -this.transform.position.z));

        Vector2 topLeft = new Vector2(topLeftView.x + difference.x, topLeftView.y + difference.y);
        Vector2 topRight = new Vector2(topRightView.x + difference.x, topRightView.y + difference.y);
        Vector2 bottomLeft = new Vector2(bottomLeftView.x + difference.x, bottomLeftView.y + difference.y);
        Vector2 bottomRight = new Vector2(bottomRightView.x + difference.x, bottomRightView.y + difference.y);

        if (this.bounds != null)
        {
            if (!this.bounds.OverlapPoint(topLeft))
            {
                if (this.bounds.OverlapPoint(topRight))
                {
                    position.x = this.transform.position.x;
                }
                else if (this.bounds.OverlapPoint(bottomLeft))
                {
                    position.y = this.transform.position.y;
                }
                else if (this.bounds.OverlapPoint(bottomRight))
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
                else
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
            }

            if (!this.bounds.OverlapPoint(topRight))
            {
                if (this.bounds.OverlapPoint(topLeft))
                {
                    position.x = this.transform.position.x;
                }
                else if (this.bounds.OverlapPoint(bottomRight))
                {
                    position.y = this.transform.position.y;
                }
                else if (this.bounds.OverlapPoint(bottomLeft))
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
                else
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
            }

            if (!this.bounds.OverlapPoint(bottomLeft))
            {
                if (this.bounds.OverlapPoint(bottomRight))
                {
                    position.x = this.transform.position.x;
                }
                else if (this.bounds.OverlapPoint(topLeft))
                {
                    position.y = this.transform.position.y;
                }
                else if (this.bounds.OverlapPoint(topRight))
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
                else
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
            }

            if (!this.bounds.OverlapPoint(bottomRight))
            {
                if (this.bounds.OverlapPoint(bottomLeft))
                {
                    position.x = this.transform.position.x;
                }
                else if (this.bounds.OverlapPoint(topRight))
                {
                    position.y = this.transform.position.y;
                }
                else if (this.bounds.OverlapPoint(topLeft))
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
                else
                {
                    position.x = this.transform.position.x;
                    position.y = this.transform.position.y;
                }
            }
        }

        return position;
    }

    private IEnumerator Wait()
    {
        this.stopped = true;

        yield return new WaitForSecondsRealtime(0.1f);

        this.stopped = false;
    }

    private struct FocusArea
    {
        [SerializeField] private Vector2 velocity;

        [SerializeField] private Vector2 center;

        public readonly Vector2 Velocity => velocity;

        public readonly Vector2 Center => center;

        private float left, right, top, bottom;

        public FocusArea(Bounds bounds, Vector2 size)
        {
            this.velocity = Vector2.zero;

            this.left = bounds.center.x - size.x / 2;
            this.right = bounds.center.x + size.x / 2;
            this.top = bounds.min.y + size.y;
            this.bottom = bounds.min.y;

            this.center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Refresh(Bounds bounds)
        {
            float shiftX = 0f;

            if (bounds.min.x < left)
            {
                shiftX = bounds.min.x - left;
            }
            else if (bounds.max.x > right)
            {
                shiftX = bounds.max.x - right;
            }

            float shiftY = 0f;

            if (bounds.min.y < bottom)
            {
                shiftY = bounds.min.y - bottom;
            }
            else if (bounds.max.y > top)
            {
                shiftY = bounds.max.y - top;
            }

            this.velocity = new Vector2(shiftX, shiftY);

            this.left += shiftX;
            this.right += shiftX;

            this.top += shiftY;
            this.bottom += shiftY;

            Vector2 newCenter = new Vector2((this.left + this.right) / 2f, (this.top + this.bottom) / 2f);

            this.center = newCenter;
        }
    }
}