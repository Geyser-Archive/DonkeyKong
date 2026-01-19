using System.Collections;
using UnityEngine;

public class Crossfade : MonoBehaviour
{
    public const float DURATION = 0.5f;

    public static Crossfade Instance => Crossfade.instance;
    private static Crossfade instance;

    private Animator animator;

    public void Load(SceneSystem.Scene sceneToLoad)
    {
        if (!base.gameObject.activeSelf)
        {
            this.SetVisible(true);
        }

        this.StartCoroutine(this.CrossfadeLoading(sceneToLoad));
    }

    public void CrossfadeIn()
    {
        this.animator.SetTrigger("Fade In");
    }

    public void CrossfadeOut()
    {
        this.animator.SetTrigger("Fade Out");
    }

    public void InOut()
    {
        this.StartCoroutine(this.CrossfadeInOut());
    }

    public void SetVisible(bool value)
    {
        this.gameObject.SetActive(value);
    }

    private void Awake()
    {
        Crossfade.instance = this;

        this.animator = base.GetComponent<Animator>();
    }

    private IEnumerator CrossfadeLoading(SceneSystem.Scene sceneToLoad)
    {
        this.CrossfadeIn();

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        SceneSystem.Load(sceneToLoad);
    }

    private IEnumerator CrossfadeInOut()
    {
        this.CrossfadeIn();

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.CrossfadeOut();
    }
}