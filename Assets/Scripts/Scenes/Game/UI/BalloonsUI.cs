using System.Collections;
using UnityEngine;

public class BalloonsUI : MonoBehaviour
{
    [SerializeField] private TextToFont textToFont;

    [SerializeField] private Animator imageAnimator;

    private Animator animator;

    private Coroutine coroutine;

    public void Refresh(int amount)
    {
        if (amount == -1)
        {
            this.textToFont.ChangeText("");
        }
        else
        {
            this.textToFont.ChangeText(amount.ToString());
        }

        if (this.coroutine != null)
        {
            base.StopCoroutine(this.coroutine);

            this.coroutine = null;
        }
        else
        {
            this.animator.SetTrigger("Fade In");
        }

        this.coroutine = base.StartCoroutine(this.HideCoroutine());
    }

    public void Death()
    {
        this.imageAnimator.SetTrigger("Death");

        base.Invoke(nameof(Hide), 0.15f);

        if (this.coroutine != null)
        {
            base.StopCoroutine(this.coroutine);

            this.coroutine = null;
        }
    }

    private void Awake()
    {
        this.animator = base.GetComponent<Animator>();
    }

    private void Hide()
    {
        this.animator.SetTrigger("Fade Out");
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);

        this.animator.SetTrigger("Fade Out");

        this.coroutine = null;
    }
}