using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerMaster))]
public class PlayerNetworkMaster : NetworkBehaviour
{
    [SerializeField] private float threshold = 0.05f;

    private SpriteRenderer spriteRenderer;

    private float x = 0f;

    private void Awake()
    {
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (base.IsOwner)
        {
            base.tag = "Player";

            GameController.Instance.Init();
        }
        else
        {
            base.GetComponent<PlayerMaster>().enabled = false;
            base.GetComponent<PlayerAnimation>().enabled = false;
            base.GetComponent<PlayerInput>().enabled = false;
            base.GetComponent<Player>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner)
        {
            float currentX = base.transform.position.x;

            float difference = currentX - this.x;

            if (difference > this.threshold || difference < -this.threshold)
            {
                this.spriteRenderer.flipX = difference < 0f;
            }

            this.x = currentX;
        }
    }
}