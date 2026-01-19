using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;

    private bool locked = false;

    public void Lock(bool value)
    {
        this.locked = value;

        if (value)
        {
            this.player.DirectionalInput(Vector2.zero);
        }
    }

    private void Awake()
    {
        this.player = base.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            this.player.SprintInputDown();
        }

        if (Input.GetButtonUp("Sprint"))
        {
            this.player.SprintInputUp();
        }

        if (this.locked)
        {
            return;
        }

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        this.player.DirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            this.player.JumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            this.player.JumpInputUp();
        }

        float speed = this.player.Speed;

        float absSpeed = Mathf.Abs(speed);

        if (absSpeed <= 5f)
        {
            if (Input.GetButtonDown("Roll"))
            {
                this.player.Roll();
            }
        }
    }
}