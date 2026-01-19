using UnityEngine;

public class ExtrasUI : MonoBehaviour
{
    [SerializeField] private GameObject cartButton;
    [SerializeField] private GameObject bossButton;

    private void Awake()
    {
        GameData gameData = Game.GameData;

        if (!gameData.LevelCompleted)
        {
            this.cartButton.GetComponent<ButtonOnAction>().Interactable = false;

            base.Invoke(nameof(this.ChangeColorCart), 1f);
        }

        if (!gameData.CartCompleted)
        {
            this.bossButton.GetComponent<ButtonOnAction>().Interactable = false;

            base.Invoke(nameof(this.ChangeColorBoss), 1f);
        }
    }

    public void Cart()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(SceneSystem.Scene.Cart);
        }
        else
        {
            SceneSystem.Load(SceneSystem.Scene.Cart);
        }
    }

    public void Boss()
    {
        if (Crossfade.Instance != null)
        {
            Crossfade.Instance.Load(SceneSystem.Scene.Boss);
        }
        else
        {
            SceneSystem.Load(SceneSystem.Scene.Boss);
        }
    }

    private void ChangeColorCart()
    {
        TextToFont textToFont = this.cartButton.GetComponentInChildren<TextToFont>();

        textToFont.ChangeColor("000128000128");
    }

    private void ChangeColorBoss()
    {
        TextToFont textToFont = this.bossButton.GetComponentInChildren<TextToFont>();

        textToFont.ChangeColor("000128000128");
    }
}