using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Raycast : MonoBehaviour
{
    public const float WIDTH = 0.015f;

    [SerializeField] private float distanceBetweenRays = 0.25f;

    public float HorizontalRaySpacing { get; private set; }
    public float VerticalRaySpacing { get; private set; }

    public BoxCollider2D BoxCollider2D => boxCollider2D;
    private BoxCollider2D boxCollider2D;

    public RaycastOrigins Origins => origins;
    private RaycastOrigins origins;

    public int HorizontalRayCount => horizontalRayCount;
    public int VerticalRayCount => verticalRayCount;
    private int horizontalRayCount;
    private int verticalRayCount;

    public virtual void Awake()
    {
        this.boxCollider2D = base.GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        this.UpdateRaycastOrigins();

        this.CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = BoxCollider2D.bounds;

        bounds.Expand(Raycast.WIDTH * -2f);

        this.origins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        this.origins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        this.origins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        this.origins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = this.boxCollider2D.bounds;

        bounds.Expand(Raycast.WIDTH * -2f);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        this.horizontalRayCount = Mathf.RoundToInt(boundsHeight / this.distanceBetweenRays);
        this.verticalRayCount = Mathf.RoundToInt(boundsWidth / this.distanceBetweenRays);

        this.HorizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        this.VerticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 TopLeft { get; set; }
        public Vector2 TopRight { get; set; }
        public Vector2 BottomLeft { get; set; }
        public Vector2 BottomRight { get; set; }

        public RaycastOrigins(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;
        }
    }
}