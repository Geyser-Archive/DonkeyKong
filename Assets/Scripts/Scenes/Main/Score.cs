using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextToFont textToFont;

    private void Start()
    {
        base.Invoke(nameof(this.Change), 2f);
    }

    private void Change()
    {
        this.textToFont.ChangeColor("255255255255");

        this.textToFont.ChangeText(Game.GameData.Score.ToString());
    }
}