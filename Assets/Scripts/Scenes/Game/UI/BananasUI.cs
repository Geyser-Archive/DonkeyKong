using System.Collections;
using UnityEngine;

public class BananasUI : MonoBehaviour
{
    [SerializeField] private TextToFont textToFont;

    private Animator animator;

    private Coroutine coroutine;

    public void Refresh(int amount)
    {
        this.textToFont.ChangeText(amount.ToString());

        if (this.coroutine != null)
        {
            base.StopCoroutine(this.coroutine);
        }
        else
        {
            this.animator.SetTrigger("Fade In");
        }

        this.coroutine = base.StartCoroutine(this.HideCoroutine());
    }

    private void Awake()
    {
        this.animator = base.GetComponent<Animator>();
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);

        this.animator.SetTrigger("Fade Out");

        this.coroutine = null;
    }
}