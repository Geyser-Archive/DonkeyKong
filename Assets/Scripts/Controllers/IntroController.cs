using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    [SerializeField] private bool autoSkip = true;

    private PlayableDirector intro;

    private bool key;

    public void Done()
    {
        if (this.autoSkip)
        {
            this.Load();
        }
    }

    public void Intro_Restart()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();

            this.StartCoroutine(this.Intro());
        }
        else
        {
            this.intro.Stop();
            this.intro.Play();
        }
    }

    private void Awake()
    {
        this.intro = base.GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.CrossfadeOut();
        }
    }

    private void Update()
    {
        if (!this.autoSkip && !this.key)
        {
            if (Input.anyKeyDown)
            {
                this.key = true;

                this.Load();
            }
        }
    }

    private void Load()
    {
        SceneSystem.Scene sceneToLoad = SceneSystem.Scene.Main;

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(sceneToLoad);
        }
        else
        {
            SceneSystem.Load(sceneToLoad);
        }
    }

    private IEnumerator Intro()
    {
        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.intro.Stop();
        this.intro.Play();
    }
}