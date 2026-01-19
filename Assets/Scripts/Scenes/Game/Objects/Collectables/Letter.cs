using System.Collections;
using UnityEngine;

public class Letter : Collectable
{
    [SerializeField] private char letter;

    private readonly Vector2 velocity = new Vector2(0f, 1f);

    private readonly float speed = 10f;

    public override void Collect()
    {
        GameController.Instance.CollectLetter(this.letter);

        base.Invoke(nameof(base.DestroyAfterCollected), 1f);

        if (base.gameObject.activeInHierarchy)
        {
            base.StartCoroutine(this.Move());
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            base.transform.Translate(this.velocity * this.speed * Time.deltaTime);

            yield return null;
        }
    }
}