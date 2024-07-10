using UnityEngine;

public static class Method
{
    public static bool IsPlayerObject(GameObject gameObject)
    {
        return gameObject.tag == "Player" ? true : false;
    }
}
