using System.Collections;
using UnityEngine;

public class EndController : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private GameObject banana, bananaBunch;

    [SerializeField] private GameObject smallController, bigController;

    [SerializeField] private Transform[] smallSpawns, bigSpawns;

    [SerializeField] private float timeSmall = 1, timeBig = 1.5f;

    [SerializeField] private int maxAmountSmall = 500, maxAmountBig = 1000;

    private Transform[] spawns;

    private float time;

    private int maxAmount;

    private int amount = 0;

    private void Awake()
    {
        bool completed = Game.GameData.Completed();

        if (completed)
        {
            this.bigController.SetActive(true);

            this.spawns = this.bigSpawns;

            this.time = this.timeBig;

            this.maxAmount = this.maxAmountBig;
        }
        else
        {
            this.smallController.SetActive(true);

            this.spawns = this.smallSpawns;

            this.time = this.timeSmall;

            this.maxAmount = this.maxAmountSmall;
        }
    }

    private void Start()
    {
        base.StartCoroutine(this.Loop());
    }

    private GameObject generate()
    {
        if (this.amount >= this.maxAmount)
        {
            return null;
        }

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            this.amount++;

            return this.banana;
        }
        else
        {
            if (this.amount + 5 > this.maxAmount)
            {
                this.amount++;

                return this.banana;
            }

            this.amount += 5;

            return this.bananaBunch;
        }
    }

    private IEnumerator Loop()
    {
        while (amount < maxAmount)
        {
            int random = Random.Range(0, 5);

            if (random == 0)
            {
                yield return new WaitForSecondsRealtime(1f);

                continue;
            }

            if (random == 1)
            {
                base.StartCoroutine(this.SpawnSmart());
            }
            else if (random == 2)
            {
                base.StartCoroutine(this.SpawnLeftOrRight());
            }
            else if (random == 3)
            {
                base.StartCoroutine(this.Spawn());
            }
            else
            {
                base.StartCoroutine(this.SpawnRandom());
            }

            yield return new WaitForSecondsRealtime(this.time);
        }

        GameController gameController = GameController.Instance;

        yield return new WaitUntil(() => gameController.Score() == this.maxAmount);

        Game.GameData.Score += GameController.Instance.Score();

        SaveLoadSystem.SaveGame();

        GameController.Instance.Leave();
    }

    private IEnumerator SpawnRandom()
    {
        for (int i = 0; i < this.spawns.Length; i++)
        {
            int random = Random.Range(0, this.spawns.Length);

            GameObject gameObject = this.generate();

            if (gameObject == null)
            {
                yield break;
            }

            Object.Instantiate(gameObject, this.spawns[random].position, Quaternion.identity);

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < this.spawns.Length; i++)
        {
            GameObject gameObject = this.generate();

            if (gameObject == null)
            {
                yield break;
            }

            Object.Instantiate(gameObject, this.spawns[i].position, Quaternion.identity);

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator SpawnLeftOrRight()
    {
        Transform[] spawns;

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            spawns = this.spawns;
        }
        else
        {
            spawns = new Transform[this.spawns.Length];

            for (int i = 0; i < this.spawns.Length; i++)
            {
                spawns[i] = this.spawns[this.spawns.Length - 1 - i];
            }
        }

        foreach (Transform spawn in spawns)
        {
            GameObject gameObject = this.generate();

            if (gameObject == null)
            {
                yield break;
            }

            Object.Instantiate(gameObject, spawn.position, Quaternion.identity);

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator SpawnSmart()
    {
        for (int i = 0; i < this.spawns.Length; i++)
        {
            Transform targetSpawn = null;

            foreach (Transform spawn in this.spawns)
            {
                Transform transform = this.target.transform;

                float targetX = transform.position.x;

                float spawnX = spawn.position.x;

                float distance = Mathf.Abs(targetX - spawnX);

                if (targetSpawn == null)
                {
                    targetSpawn = spawn;
                }
                else
                {
                    float targetSpawnX = targetSpawn.position.x;

                    float targetSpawnDistance = Mathf.Abs(targetX - targetSpawnX);

                    if (distance < targetSpawnDistance)
                    {
                        targetSpawn = spawn;
                    }
                }
            }

            GameObject gameObject = this.generate();

            if (gameObject == null)
            {
                yield break;
            }

            Object.Instantiate(gameObject, targetSpawn.position, Quaternion.identity);

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}