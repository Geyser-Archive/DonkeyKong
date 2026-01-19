using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private GameObject location;

    [SerializeField] private GameObject mainCamera;

    private Vector3 initPosition;

    public void Unload()
    {
        this.location.SetActive(false);
    }

    public void Load()
    {
        this.location.SetActive(true);
    }

    public void InitLoad()
    {
        this.mainCamera.transform.position = initPosition;

        this.location.SetActive(true);
    }

    private void Awake()
    {
        this.initPosition = mainCamera.transform.position;
    }
}