using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private FrameRate frameRate;

    private bool changes = false;

    private void Start()
    {
        this.frameRate = new FrameRate(Settings.SettingsData.FrameRate);
    }

    public void ChangeFrameRate()
    {
        this.changes = true;

        this.frameRate.Next();

        Settings.SettingsData.FrameRate = this.frameRate.Value;

        Application.targetFrameRate = Settings.SettingsData.FrameRate;
    }

    public void Save()
    {
        if (!this.changes)
        {
            return;
        }

        this.changes = false;

        SaveLoadSystem.SaveSettings();
    }

    private struct FrameRate
    {
        public int Value => this.value;

        private int value;

        public FrameRate(int value)
        {
            this.value = value;
        }

        public void Next()
        {
            if (this.value == 0)
            {
                this.value = 30;
            }
            else if (this.value == 30)
            {
                this.value = 60;
            }
            else if (this.value == 60)
            {
                this.value = 120;
            }
            else if (this.value == 120)
            {
                this.value = 240;
            }
            else if (this.value == 240)
            {
                this.value = 0;
            }
        }
    }
}