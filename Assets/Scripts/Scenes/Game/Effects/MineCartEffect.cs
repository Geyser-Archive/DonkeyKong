using UnityEngine;

public class MineCartEffect : MonoBehaviour
{
    private void Start()
    {
        base.Invoke(nameof(Destroy), 1f);
    }

    private void Destroy()
    {
        Object.Destroy(base.gameObject);
    }
}