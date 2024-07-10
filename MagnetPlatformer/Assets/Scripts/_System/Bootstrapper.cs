using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Object systems = Object.Instantiate(Resources.Load("Systems"));
        Object.DontDestroyOnLoad(systems);
        systems.name = "Systems";
    }
}