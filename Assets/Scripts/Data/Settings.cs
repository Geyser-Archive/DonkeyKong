public static class Settings
{
    private static SettingsData settingsData;

    public static SettingsData SettingsData
    {
        get
        {
            if (Settings.settingsData == null)
            {
                Settings.LoadDefaultSettings();
            }

            return Settings.settingsData;
        }
        set
        {
            Settings.settingsData = value;
        }
    }

    public static void LoadDefaultSettings()
    {
        Settings.settingsData = new SettingsData();

        Settings.settingsData.FrameRate = 0; // 0 = Unlimited
    }
}