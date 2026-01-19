using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("DontDestroyOnLoad").Length > 1)
        {
            Object.Destroy(GameObject.FindGameObjectsWithTag("DontDestroyOnLoad")[0]);
        }

        Object.DontDestroyOnLoad(base.gameObject);
    }
}