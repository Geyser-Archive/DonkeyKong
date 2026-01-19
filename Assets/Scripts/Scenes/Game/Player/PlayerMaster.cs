using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerAnimation), typeof(BoxCollider2D))]
public class PlayerMaster : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerAnimation playerAnimation;
    private BoxCollider2D boxCollider2D;

    public void Lock(bool value)
    {
        this.playerInput.Lock(value);
    }

    public void Death()
    {
        this.playerAnimation.Death();

        this.boxCollider2D.enabled = false;
    }

    public void Revive()
    {
        this.playerAnimation.Revive();

        this.boxCollider2D.enabled = true;
    }

    private void Awake()
    {
        this.playerInput = base.GetComponent<PlayerInput>();
        this.playerAnimation = base.GetComponent<PlayerAnimation>();
        this.boxCollider2D = base.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        PlayerNetworkMaster playerNetworkMaster = base.GetComponent<PlayerNetworkMaster>();

        if (playerNetworkMaster == null)
        {
            GameController.Instance.Init();
        }
    }
}