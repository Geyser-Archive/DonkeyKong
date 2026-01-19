using System.Collections;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerUI : MonoBehaviour
{
    private const string DEFAULT_HOST = "geyser.sytes.net";

    private static readonly string defaultIp = Dns.GetHostAddresses(MultiplayerUI.DEFAULT_HOST)[0].ToString();

    [SerializeField] private GameObject startButton;
    [SerializeField] private Animator clientAnimator, lobbyAnimator;
    [SerializeField] private CanvasGroup canvasGroup;

    private Animator animator;

    private UnityTransport unityTransport;

    public void Host()
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        NetworkManager.Singleton.OnServerStarted += this.OnServerStarted;

        NetworkManager.Singleton.StartHost();
    }

    public void Server()
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        NetworkManager.Singleton.OnServerStarted += this.OnServerStarted;

        NetworkManager.Singleton.StartServer();
    }

    public void Join(TMP_InputField text)
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (string.IsNullOrEmpty(text.text))
        {
            text.text = MultiplayerUI.defaultIp;
        }

        this.unityTransport.ConnectionData.Address = text.text;

        NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnected;

        NetworkManager.Singleton.StartClient();
    }

    public void Stop()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceneSystem.Scene.Multiplayer.ToString(), LoadSceneMode.Single);
    }

    public void Leave()
    {
        NetworkManager.Singleton.Shutdown();

        this.lobbyAnimator.SetTrigger("Fade Out");
        this.animator.SetTrigger("Fade In");
    }

    private void Awake()
    {
        this.animator = base.GetComponent<Animator>();
    }

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        this.unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        // Default
        // this.unityTransport.ConnectionData.ServerListenAddress = "127.0.0.1";
        // this.unityTransport.ConnectionData.Port = 7777;
    }
    private void OnServerStarted()
    {
        NetworkManager.Singleton.OnServerStarted -= this.OnServerStarted;

        this.startButton.SetActive(true);

        this.StartCoroutine(this.Block());

        base.Invoke(nameof(this.FadeServer), 0.25f);
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnected;

        if (this.startButton == null)
        {
            return;
        }

        this.startButton.SetActive(false);

        this.StartCoroutine(this.Block());

        base.Invoke(nameof(this.FadeClient), 0.25f);
    }

    private void FadeServer()
    {
        this.animator.SetTrigger("Fade Out");
        this.lobbyAnimator.SetTrigger("Fade In");
    }

    private void FadeClient()
    {
        this.clientAnimator.SetTrigger("Fade Out");
        this.lobbyAnimator.SetTrigger("Fade In");
    }

    private IEnumerator Block()
    {
        this.canvasGroup.blocksRaycasts = true;

        yield return new WaitForSecondsRealtime(0.5f);

        this.canvasGroup.blocksRaycasts = false;
    }
}