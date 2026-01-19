using UnityEngine;

[RequireComponent(typeof(MineCartInput))]
public class MineCartPassenger : MonoBehaviour
{
    [SerializeField] private GameObject passenger;

    [SerializeField] private bool lockOnEnter = false;
    [SerializeField] private float lockTime;

    private MineCartInput mineCartInput;

    private Player player;

    private bool locked = false;

    private bool trigger = false;

    public void Passenger(GameObject passenger)
    {
        Player player = passenger.GetComponent<Player>();

        bool success = player.Mount(this.transform);

        if (!success)
        {
            return;
        }

        this.player = player;

        this.mineCartInput.Activate();

        this.passenger.SetActive(true);
    }

    public void Die()
    {
        if (this.player == null)
        {
            return;
        }

        this.Leave();

        if (GameController.Instance != null)
        {
            GameController.Instance.Die();
        }
    }

    private void Awake()
    {
        this.mineCartInput = base.GetComponent<MineCartInput>();
    }

    private void Update()
    {
        if (this.player == null)
        {
            return;
        }

        if (!this.trigger)
        {
            this.trigger = true;

            if (this.lockOnEnter)
            {
                this.locked = true;

                base.Invoke(nameof(this.Unlock), this.lockTime);
            }

            return;
        }

        if (this.locked)
        {
            return;
        }

        float vertical = Input.GetAxisRaw("Vertical");

        this.mineCartInput.Vertical(vertical);

        if (Input.GetButtonDown("Jump"))
        {
            this.mineCartInput.JumpDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            this.mineCartInput.JumpUp();
        }

        if (Input.GetButtonDown("Sprint"))
        {
            if (this.player == null)
            {
                return;
            }

            this.Leave();
        }
    }

    private void Leave()
    {
        this.passenger.SetActive(false);

        this.mineCartInput.Vertical(0f);

        this.player.Dismount();

        this.player.BounceMax();

        this.player = null;

        this.trigger = false;
    }

    private void Unlock()
    {
        this.locked = false;
    }
}