using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance => GameController.instance;
    private static GameController instance;

    [SerializeField] private SceneSystem.Scene debugScene = SceneSystem.Scene.None;

    [SerializeField] private GameObject player;
    [SerializeField] private Stage mainStage;
    [SerializeField] private Transform spawn;

    [SerializeField] private BalloonsUI balloonsUI;
    [SerializeField] private BananasUI bananasUI;
    [SerializeField] private LettersUI lettersUI;

    private PlayerMaster playerMaster;

    private Stage activeStage;

    private int balloons = 0;
    private int bananas = 0;
    private int letters = 0;

    private bool initiated = false;

    public void Init()
    {
        if (this.player == null)
        {
            this.player = GameObject.FindGameObjectWithTag("Player");
        }

        this.playerMaster = this.player.GetComponent<PlayerMaster>();

        this.ChangeStage(this.mainStage);

        this.Spawn(this.spawn.position);

        this.initiated = true;
    }

    public void ChangeStage(Stage stage)
    {
        if (this.activeStage != null)
        {
            this.activeStage.Unload();
        }

        this.activeStage = stage;

        this.activeStage.Load();
    }

    public void Spawn(Vector3 position)
    {
        if (this.player == null)
        {
            return;
        }

        this.player.transform.position = position;
    }

    public void Die()
    {
        this.balloons--;

        this.balloonsUI.Refresh(this.balloons);

        this.balloonsUI.Death();

        this.playerMaster.Lock(true);

        this.playerMaster.Death();

        base.Invoke(nameof(this.HandleDeath), 2f);
    }

    public void CollectBananas(int amount)
    {
        this.bananas += amount;

        this.bananasUI.Refresh(this.bananas);
    }

    public void CollectBalloons(int amount)
    {
        this.balloons += amount;

        this.balloonsUI.Refresh(this.balloons);
    }

    public void CollectLetter(char letter)
    {
        this.letters++;

        this.lettersUI.Refresh(letter);
    }

    public void Lock(bool value)
    {
        this.playerMaster.Lock(value);
    }

    public void Leave()
    {
        this.playerMaster.Lock(true);

        this.Stop();

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

    public void LeaveEnd()
    {
        this.playerMaster.Lock(true);

        this.Stop();

        SceneSystem.Scene sceneToLoad = SceneSystem.Scene.End;

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(sceneToLoad);
        }
        else
        {
            SceneSystem.Load(sceneToLoad);
        }
    }

    public bool Level100Percent()
    {
        int maxBalloons = 6;
        int maxBananas = 40;
        int maxLetters = 4;

        int maxEnemies = 14;

        bool balloonsCompleted = this.balloons == maxBalloons;
        bool bananasCompleted = this.bananas == maxBananas;
        bool lettersCompleted = this.letters == maxLetters;

        int kills = EnemyManager.Instance.Kills;

        bool enemiesCompleted = kills == maxEnemies;

        bool completed = balloonsCompleted && bananasCompleted && lettersCompleted && enemiesCompleted;

        return completed;
    }

    public int Score()
    {
        int score = 0;

        score += this.balloons * 10;
        score += this.bananas * 1;
        score += this.letters * 25;

        return score;
    }

    private void Awake()
    {
        GameController.instance = this;
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
        if (Input.GetButtonDown("Cancel"))
        {
            if (this.playerMaster != null)
            {
                this.Leave();
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (this.debugScene != SceneSystem.Scene.None)
            {
                SceneSystem.Scene sceneToLoad = this.debugScene;

                if (Crossfade.Instance != null)
                {
                    Crossfade.Instance.Load(sceneToLoad);
                }
                else
                {
                    SceneSystem.Load(sceneToLoad);
                }
            }
        }

        if (this.initiated)
        {
            if (this.player == null)
            {
                this.Leave();
            }
        }
    }

    private void Stop()
    {
        if (NetworkController.Instance != null)
        {
            NetworkController.Instance.Stop();
        }
    }

    private void HandleDeath()
    {
        if (this.balloons < 0)
        {
            this.GameOver();
        }
        else
        {
            this.StartCoroutine(this.Respawn());
        }
    }

    private void GameOver()
    {
        this.Stop();

        SceneSystem.Scene sceneToLoad = SceneSystem.Scene.GameOver;

        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(sceneToLoad);
        }
        else
        {
            if (this.debugScene != SceneSystem.Scene.None)
            {
                sceneToLoad = this.debugScene;
            }

            SceneSystem.Load(sceneToLoad);
        }
    }

    private IEnumerator Respawn()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.InOut();
        }

        yield return new WaitForSecondsRealtime(Crossfade.DURATION);

        this.playerMaster.Lock(false);

        this.playerMaster.Revive();

        if (this.activeStage != this.mainStage)
        {
            this.activeStage.Unload();

            this.activeStage = this.mainStage;
        }

        this.activeStage.InitLoad();

        this.Spawn(this.spawn.position);
    }
}