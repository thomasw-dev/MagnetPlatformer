using System.Collections.Generic;

public static class Constants
{
    public const float CAMERA_FOLLOW_SMOOTH_FACTOR = 4f;

    public enum ENUM_TAG
    {
        ENVIRONMENT,
        MAGNETIC_OBJECT,
        ENEMY,
        ENEMY_BOSS
    };

    public static Dictionary<ENUM_TAG, string> TAG = new Dictionary<ENUM_TAG, string>
    {
        { ENUM_TAG.ENVIRONMENT, "Environment" },
        { ENUM_TAG.MAGNETIC_OBJECT, "Magnetic Object" },
        { ENUM_TAG.ENEMY, "Enemy" },
        { ENUM_TAG.ENEMY_BOSS, "Enemy Boss" }
    };

    public enum LAYER
    {
        Physics = 3,
        Camera = 6,
        Player = 7,
        Environment = 8,
        Magnetic = 9
    }

    // Scriptable Objects: Create Asset Menu Orders
    public const int GIZMOS_SETTINGS_CONTROL = 1;
    public const int MAGNET_OBJECT_GROUP = 2;
    public const int MAGNETIC_OBJECT_SPRITE_SET = 3;
    public const int MAGNETIC_OBJECT_SPRITE_SET_COLLECTION = 4;
}