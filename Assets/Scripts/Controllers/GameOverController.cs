using System.Collections;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private float ambienceTime;

    [SerializeField] private AudioSource ambience;

    private bool wait = true;

    private void Start()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.CrossfadeOut();
        }

        base.Invoke(nameof(this.Wait), 1f);

        base.StartCoroutine(this.WaitForAmbience());
    }

    private void Update()
    {
        if (this.wait)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            this.Load();
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

    private void Wait()
    {
        this.wait = false;
    }

    private IEnumerator WaitForAmbience()
    {
        yield return new WaitForSecondsRealtime(this.ambienceTime);

        this.ambience.Play();

        yield return new WaitForSecondsRealtime(this.ambience.clip.length);

        this.Load();
    }
}