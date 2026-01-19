[System.Serializable]
public class GameData
{
    public int Score
    {
        get { return this.score; }
        set
        {
            if (value > int.MaxValue)
            {
                this.score = int.MaxValue;
            }
            else { this.score = value; }
        }
    }

    private int score = 0;

    public bool LevelCompleted { get; set; }
    public bool CartCompleted { get; set; }
    public bool BossCompleted { get; set; }

    public bool Level100Percent { get; set; }

    public bool Completed()
    {
        return this.LevelCompleted && this.CartCompleted && this.BossCompleted && this.Level100Percent;
    }
}