using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossInput : MonoBehaviour
{
    public Boss Boss => this.boss;
    private Boss boss;

    public bool Locked => this.locked;
    private bool locked = false;

    public void Lock(bool value)
    {
        this.locked = value;

        if (value)
        {
            this.boss.DirectionalInput(Vector2.zero);
        }
    }

    public void Jump()
    {
        this.boss.Bounce(4f);
    }

    private void Awake()
    {
        this.boss = base.GetComponent<Boss>();
    }
}