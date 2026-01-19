using TMPro;
using UnityEngine;

public class CustomInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextToFont textToFont;
    [SerializeField] private int maxLength = 20;

    public void Changed()
    {
        string text = this.inputField.text;

        if (text.Length > maxLength)
        {
            this.inputField.text = text[..maxLength];

            return;
        }

        this.textToFont.ChangeText(this.inputField.text);
    }
}