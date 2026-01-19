using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private PolygonCollider2D bounds;
    [SerializeField] private float verticalSmoothTime = 0.2f;
    [SerializeField] private float verticalOffset = 0f;

    private Camera myCamera;

    private float smoothVelocityY;

    private bool stopped;

    private void Awake()
    {
        this.myCamera = base.GetComponent<Camera>();
    }

    private void Start()
    {
        this.stopped = false;
    }

    private void LateUpdate()
    {
        if (this.stopped)
        {
            return;
        }

        if (this.target == null)
        {
            return;
        }

        Vector2 focusPosition = this.target.transform.position + Vector3.up * this.verticalOffset;

        focusPosition.y = Mathf.SmoothDamp(this.transform.position.y, focusPosition.y, ref this.smoothVelocityY, this.verticalSmoothTime);

        Vector3 newPosition = (Vector3)focusPosition + Vector3.forward * -10f;

        this.Apply(newPosition);
    }

    private void OnEnable()
    {
        base.StartCoroutine(this.Wait());
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
}