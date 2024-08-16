public static class Magnet
{
    public enum Charge { Neutral, Positive, Negative }
}

public static class MagneticObject
{
    public enum Type { Free, Vertical, Horizontal, Static }
}

public static class Move
{
    public enum Direction { None, Left, Right }
}

public static class CameraArea
{
    public enum Mode { FollowPlayer, Horizontal, Vertical, Fixed }
}

public static class Direction
{
    public enum Type { Vertical, Horizontal }
    public enum Side
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3
    }
}