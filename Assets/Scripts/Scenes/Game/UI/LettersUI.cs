using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LettersUI : MonoBehaviour
{
    [SerializeField] private Image[] images;

    private Animator animator;

    private Coroutine coroutine;

    public void Refresh(char letter)
    {
        int value = this.Convert(letter);

        if (value == -1)
        {
            return;
        }

        this.images[value].enabled = true;

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

    private int Convert(char letter)
    {
        return letter switch
        {
            'K' => 0,
            'O' => 1,
            'N' => 2,
            'G' => 3,
            _ => -1,
        };
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);

        this.animator.SetTrigger("Fade Out");

        this.coroutine = null;
    }
}