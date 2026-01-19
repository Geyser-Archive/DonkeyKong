using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private GameObject title, background;

    [SerializeField] private GameObject[] slides;

    [SerializeField] private Animator mainAnimator, uiAnimator;

    private Coroutine coroutine;

    private float time = 0f;

    private void Start()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.CrossfadeOut();
        }

        this.StartCoroutine(this.Title());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (this.time == -1f)
            {
                base.StopCoroutine(this.coroutine);

                base.StartCoroutine(this.CleanUp());
            }
            else
            {
                this.time = 0f;
            }
        }

        if (this.time == -1f)
        {
            return;
        }

        this.time += Time.deltaTime;

        if (this.time >= 30f)
        {
            this.time = -1f;

            this.coroutine = base.StartCoroutine(this.Slideshow());
        }
    }

    private void ApplicationFocus(bool value)
    {
        if (value)
        {
            this.uiAnimator.SetTrigger("Fade In");
        }
        else
        {
            this.uiAnimator.SetTrigger("Fade Out");
        }
    }

    private IEnumerator Title()
    {
        this.title.SetActive(true);
        this.background.SetActive(false);

        yield return new WaitForSecondsRealtime(1.5f);

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();
        }

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.title.SetActive(false);
        this.background.SetActive(true);

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.mainAnimator.SetTrigger("Fade In");
    }

    private IEnumerator Slideshow()
    {
        this.ApplicationFocus(false);

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();
        }

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.background.SetActive(false);

        int rounds = 0;

        while (rounds < 6)
        {
            for (int i = 0; i < this.slides.Length; i++)
            {
                this.slides[i].SetActive(true);

                yield return new WaitForSecondsRealtime(Crossfade.DURATION);

                yield return new WaitForSecondsRealtime(10f);

                if (Crossfade.Instance != null)
                {
                    Crossfade.Instance.InOut();
                }

                yield return new WaitForSecondsRealtime(Crossfade.DURATION);

                this.slides[i].SetActive(false);
            }

            rounds++;
        }

        base.StartCoroutine(this.CleanUp());
    }

    private IEnumerator CleanUp()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();
        }

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        for (int i = 0; i < this.slides.Length; i++)
        {
            this.slides[i].SetActive(false);
        }

        this.background.SetActive(true);

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.time = 0f;

        this.ApplicationFocus(true);
    }
}