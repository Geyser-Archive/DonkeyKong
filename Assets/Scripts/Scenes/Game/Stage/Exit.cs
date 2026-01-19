using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private Type type;

    private enum Type
    {
        Level,
        MineCart,
        Boss
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (this.type)
            {
                case Type.Level:
                    if (!Game.GameData.LevelCompleted)
                    {
                        Game.GameData.LevelCompleted = true;
                    }

                    if (!Game.GameData.Level100Percent)
                    {

                        if (GameController.Instance.Level100Percent())
                        {
                            Game.GameData.Level100Percent = true;
                        }
                    }

                    break;
                case Type.MineCart:
                    if (!Game.GameData.CartCompleted)
                    {
                        Game.GameData.CartCompleted = true;
                    }

                    break;
                case Type.Boss:
                    if (!Game.GameData.BossCompleted)
                    {
                        Game.GameData.BossCompleted = true;
                    }

                    GameController.Instance.LeaveEnd();

                    return;
            }

            Game.GameData.Score += GameController.Instance.Score();

            SaveLoadSystem.SaveGame();

            GameController.Instance.Leave();
        }
    }
}