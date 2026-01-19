using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.Die();
            }
            else
            {
                other.transform.position = new Vector3(0f, 0f, 0f);
            }
        }

        if (other.TryGetComponent(out EnemyCollision enemyCollision))
        {
            enemyCollision.Trigger();
        }

        if (other.TryGetComponent(out MineCartCollision mineCartCollision))
        {
            mineCartCollision.Trigger();
        }
    }
}