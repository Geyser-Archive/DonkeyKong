using UnityEngine;
using System;

public class FPS : MonoBehaviour
{
    public static FPS Instance => FPS.instance;
    private static FPS instance;

    [SerializeField] private TextToFont textToFont;

    [SerializeField] private float refreshRate = 0.5f;

    private int frameCounter;
    private float time;

    public void ChangeRefreshRate(float value)
    {
        this.refreshRate = value;
    }

    public void DisplayFPS(bool value)
    {
        base.gameObject.SetActive(value);
    }

    private void Awake()
    {
        FPS.instance = this;
    }

    private void Update()
    {
        this.frameCounter++;
        this.time += Time.deltaTime;

        if (this.time >= this.refreshRate)
        {
            String text = ((int)(this.frameCounter / this.time)).ToString();

            this.textToFont.ChangeText(text);

            this.frameCounter = 0;
            this.time = 0f;
        }
    }
}