using UnityEngine;

[CreateAssetMenu(fileName = "Magnet Sprite Set Collection", order = Constants.MAGNET_SPRITE_SET_COLLECTION)]
public class MagnetSpriteSetCollection : ScriptableObject
{
    public MagnetSpriteSet Free;
    public MagnetSpriteSet Vertical;
    public MagnetSpriteSet Horizontal;
    public MagnetSpriteSet Static;

    public MagnetSpriteSet GetSpriteSetByType(MagneticObject.Type type)
    {
        return type switch
        {
            MagneticObject.Type.Free => Free,
            MagneticObject.Type.Vertical => Vertical,
            MagneticObject.Type.Horizontal => Horizontal,
            MagneticObject.Type.Static => Static,
            _ => Free
        };
    }
}
