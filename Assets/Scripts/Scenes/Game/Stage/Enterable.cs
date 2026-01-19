using System.Collections;
using UnityEngine;

public class Enterable : MonoBehaviour
{
    [SerializeField] private Stage stage;

    [SerializeField] private Transform spawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.StartCoroutine(this.Switch());
        }
    }

    private IEnumerator Switch()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();
        }

        GameController.Instance.Lock(true);

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        GameController.Instance.ChangeStage(this.stage);

        GameController.Instance.Spawn(this.spawn.position);

        GameController.Instance.Lock(false);
    }
}