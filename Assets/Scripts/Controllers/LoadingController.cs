using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool forceAnimation = false;
    [SerializeField, Range(1f, 5f)] private float waitTime = 1f;

    private AsyncOperation asyncOperation;

    private bool done = false;

    private void Start()
    {
        this.StartCoroutine(this.Loading());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (this.done)
            {
                return;
            }

            Time.timeScale = 4f;
        }
    }

    private IEnumerator Loading()
    {
        this.asyncOperation = SceneManager.LoadSceneAsync((int)SceneSystem.SceneToLoad);

        this.asyncOperation.allowSceneActivation = false;

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.SetVisible(false);
        }

        yield return new WaitForEndOfFrame();

        if (asyncOperation.progress / 0.9f != 1f || this.forceAnimation)
        {
            this.FadeIn();

            yield return new WaitUntil(() => this.asyncOperation.progress / 0.9f == 1f);

            yield return new WaitForSeconds(this.waitTime);

            this.FadeOut();

            yield return new WaitForSeconds(1f);
        }

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.SetVisible(true);
        }

        this.done = true;

        Time.timeScale = 1f;

        this.asyncOperation.allowSceneActivation = true;
    }

    private void FadeIn()
    {
        this.animator.SetTrigger("Fade In");
    }


    private void FadeOut()
    {
        this.animator.SetTrigger("Fade Out");
    }
}