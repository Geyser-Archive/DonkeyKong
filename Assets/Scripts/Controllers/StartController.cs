using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class StartController : MonoBehaviour
{
    [SerializeField] private NetworkPrefabsList networkPrefabsList;

    private void Awake()
    {
        var prefabs = this.networkPrefabsList.PrefabList.Select(x => x.Prefab);

        foreach (var prefab in prefabs)
        {
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }

    private void Start()
    {
        Game.GameData = SaveLoadSystem.LoadGame();
        Settings.SettingsData = SaveLoadSystem.LoadSettings();

        bool isFirstGame = Game.IsFirstGame();

        // Applying settings after checking if it's the first game
        // When also applying Game information and doing it before checking if it's the first game
        // -> isFirstGame will always be false because GameData Property will be assigned

        this.ApplySettings();

        if (isFirstGame)
        {
            SaveLoadSystem.SaveGame();

            SceneSystem.Load(SceneSystem.Scene.Intro);
        }
        else
        {
            SceneSystem.Load(SceneSystem.Scene.Main);
        }
    }

    private void ApplySettings()
    {
        Application.targetFrameRate = Settings.SettingsData.FrameRate;
    }
}