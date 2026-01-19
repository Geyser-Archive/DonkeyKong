using UnityEngine;

public class SingleplayerUI : MonoBehaviour
{
    [SerializeField] private GameObject extrasButton;

    private void Awake()
    {
        GameData gameData = Game.GameData;

        if (!gameData.LevelCompleted)
        {
            this.extrasButton.GetComponent<ButtonOnAction>().Interactable = false;

            base.Invoke(nameof(this.ChangeColor), 1f);
        }
    }

    public void Play()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(SceneSystem.Scene.Singleplayer);
        }
        else
        {
            SceneSystem.Load(SceneSystem.Scene.Singleplayer);
        }
    }

    private void ChangeColor()
    {
        TextToFont textToFont = this.extrasButton.GetComponentInChildren<TextToFont>();

        textToFont.ChangeColor("000128000128");
    }
}