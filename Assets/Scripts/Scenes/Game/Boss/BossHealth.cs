using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth = 3;

    public int InitialHealth => this.initialHealth;
    public int Health => this.health;

    private int health;

    public void Damage(int damage)
    {
        this.health -= damage;
    }

    public bool IsDead()
    {
        return this.health <= 0;
    }

    private void Awake()
    {
        this.health = this.initialHealth;
    }
}