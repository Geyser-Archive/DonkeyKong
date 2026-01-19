using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get { return instance; } }

    private static EnemyManager instance;

    public int Kills { get { return kills; } }

    private int kills = 0;

    public void Kill()
    {
        this.kills++;
    }

    private void Awake()
    {
        EnemyManager.instance = this;
    }
}