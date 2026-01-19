using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float strength = 10f;

    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                bool success = player.Mount(this.transform);

                if (success)
                {
                    base.StartCoroutine(this.Launch(player));
                }
            }
        }
    }

    private IEnumerator Launch(Player player)
    {
        if (this.animator != null)
        {
            this.animator.SetTrigger("Launch");
        }

        yield return new WaitForSecondsRealtime(1f);

        player.Dismount();

        Quaternion rotation = this.transform.rotation;

        Vector2 direction = rotation * Vector2.up;

        player.Launch(direction, this.strength);

        if (player.TryGetComponent<PlayerAnimation>(out var playerAnimation))
        {
            playerAnimation.Roll();
        }
    }
}