using UnityEngine;
using UnityEngine.UI;

public class TextToFont : MonoBehaviour
{
    [SerializeField] private CustomFont customFont;

    [SerializeField] private GameObject container;

    [SerializeField] private Color defaultColor;

    [SerializeField] private string defaultText;

    [SerializeField] private float scale = 1f, size = 0f;
    [SerializeField] private bool containerize = false, unisize = false;

    private Color color;

    private string text;

    public void ChangeColor(string color)
    {
        Color newColor = new Color(
            float.Parse(color.Substring(0, 3)) / 255f,
            float.Parse(color.Substring(3, 3)) / 255f,
            float.Parse(color.Substring(6, 3)) / 255f,
            float.Parse(color.Substring(9, 3)) / 255f
        );

        this.ChangeColor(newColor);
    }

    public void ChangeColor(Color color)
    {
        if (this.color == color)
        {
            return;
        }

        this.color = color;

        this.Draw();
    }

    public void ChangeText(string text)
    {
        if (this.text == text)
        {
            return;
        }

        this.text = text;

        this.Draw();
    }

    public void Reset()
    {
        this.color = this.defaultColor;

        this.text = this.defaultText;

        this.Draw();
    }

    private void Start()
    {
        this.color = this.defaultColor;

        this.text = this.defaultText;

        this.Draw();
    }

    private void Draw()
    {
        foreach (Transform child in this.container.transform)
        {
            Object.Destroy(child.gameObject);
        }

        for (int i = 0; i < text.Length; i++)
        {
            char letter = text[i];

            Sprite sprite = this.FindSprite(letter);

            Vector3 localScale = Vector3.one * this.scale;

            this.container.transform.localScale = localScale;

            Transform parent = this.container.transform;

            if (this.containerize)
            {
                GameObject container = new GameObject();

                container.transform.SetParent(this.container.transform);

                container.name = letter.ToString() + " Container";

                container.transform.localScale = Vector3.one;

                RectTransform containerRectTransform = container.AddComponent<RectTransform>();

                if (this.unisize)
                {
                    containerRectTransform.sizeDelta = new Vector2(size, size);
                }
                else
                {
                    containerRectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
                }

                parent = container.transform;
            }

            GameObject gameObject = new GameObject();

            gameObject.transform.SetParent(parent);

            gameObject.name = letter.ToString();

            gameObject.transform.localScale = Vector3.one;

            Image image = gameObject.AddComponent<Image>();

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (sprite != null)
            {
                rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);

                image.sprite = sprite;

                image.color = this.color;
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(this.size, this.size);

                image.color = Color.clear;
            }
        }
    }

    private Sprite FindSprite(char letter)
    {
        int index = -1;

        if (letter >= 'A' && letter <= 'Z')
        {
            index = letter - 'A' + this.customFont.Upper;
        }
        else if (letter >= 'a' && letter <= 'z')
        {
            index = letter - 'a' + this.customFont.Lower;
        }
        else if (letter >= '0' && letter <= '9')
        {
            index = letter - '0' + this.customFont.Number;
        }

        if (index != -1 && index < this.customFont.Letters.Length)
        {
            return this.customFont.Letters[index];
        }

        return null;
    }
}