using UnityEngine.SceneManagement;

public static class SceneSystem
{
    public enum Scene
    {
        Start, Loading, Intro, Main, Singleplayer, Multiplayer, GameOver, Cart, Boss, End, None
    }

    public static Scene SceneToLoad => sceneToLoad;

    private static Scene sceneToLoad;

    public static void Load(Scene sceneToLoad)
    {
        SceneSystem.sceneToLoad = sceneToLoad;

        SceneManager.LoadScene((int)Scene.Loading);
    }
}