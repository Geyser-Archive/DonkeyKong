using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyInput : MonoBehaviour
{
    [SerializeField] private bool checkForFalling = false;

    private Enemy enemy;

    private bool alternative = false;

    private bool locked = false;

    public void ActivateAlternative()
    {
        this.alternative = true;
    }

    public void Lock(bool value)
    {
        this.locked = value;

        if (value)
        {
            this.enemy.DirectionalInput(Vector2.zero);
        }
    }

    private void Awake()
    {
        this.enemy = base.GetComponent<Enemy>();
    }

    private void Update()
    {
        if (this.locked)
        {
            return;
        }

        EnemyController2D enemyController2D = this.enemy.EnemyController2D;

        bool wouldFall = false;

        if (this.checkForFalling)
        {
            wouldFall = enemyController2D.WouldFall;
        }

        Vector2 directionalInput;

        if (enemyController2D.FaceDirection() == -1)
        {
            if (enemyController2D.Left() || wouldFall)
            {
                directionalInput = new Vector2(1f, 0f);
            }
            else
            {
                directionalInput = new Vector2(-1f, 0f);
            }
        }
        else
        {
            if (enemyController2D.Right() || wouldFall)
            {
                directionalInput = new Vector2(-1f, 0f);
            }
            else
            {
                directionalInput = new Vector2(1f, 0f);
            }
        }

        this.enemy.DirectionalInput(directionalInput);
    }

    private void OnBecameVisible()
    {
        if (this.alternative)
        {
            return;
        }

        this.locked = false;
    }

    private void OnBecameInvisible()
    {
        if (this.alternative)
        {
            return;
        }

        this.locked = true;

        this.enemy.DirectionalInput(Vector2.zero);
    }
}