public static class Game
{
    private static GameData gameData;

    public static GameData GameData
    {
        get
        {
            if (Game.gameData == null)
            {
                Game.LoadDefaultGame();
            }

            return Game.gameData;
        }
        set
        {
            Game.gameData = value;
        }
    }

    public static bool IsFirstGame()
    {
        return Game.gameData == null;
    }

    public static void LoadDefaultGame()
    {
        Game.gameData = new GameData();

        Game.gameData.Score = 0;

        Game.gameData.LevelCompleted = false;
        Game.gameData.CartCompleted = false;
        Game.gameData.BossCompleted = false;

        Game.gameData.Level100Percent = false;
    }
}