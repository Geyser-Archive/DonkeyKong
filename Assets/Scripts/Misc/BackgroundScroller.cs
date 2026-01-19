using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;

    private Material material;

    private void Awake()
    {
        this.material = base.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        Vector2 offset = this.material.mainTextureOffset;

        offset.x += this.speed * Time.deltaTime;

        this.material.mainTextureOffset = offset;

        if (offset.x >= 1f)
        {
            offset.x = 0f;
        }
    }

    /*
    private float offset;

    private void Update()
    {
        this.offset += this.speed * Time.deltaTime;

        Vector2 offset = new Vector2(this.offset, 0f);

        this.material.mainTextureOffset = offset;

        if (this.offset.x >= 1f)
        {
            this.offset.x = 0f;
        }
    }
    */
}