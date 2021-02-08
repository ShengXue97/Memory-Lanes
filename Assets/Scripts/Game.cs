using UnityEngine;

public class Game : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
        GameObject game = Instantiate(Resources.Load<GameObject>("Game"));
        DontDestroyOnLoad(game);
    }
}
