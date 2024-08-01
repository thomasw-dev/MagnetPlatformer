using UnityEngine;

[CreateAssetMenu(fileName = "Magnetic Object Sprite Set Collection", order = Constants.MAGNETIC_OBJECT_SPRITE_SET_COLLECTION)]
public class MagneticObjectSpriteSetCollection : ScriptableObject
{
    public MagneticObjectSpriteSet Free;
    public MagneticObjectSpriteSet Vertical;
    public MagneticObjectSpriteSet Horizontal;
    public MagneticObjectSpriteSet Static;

    public MagneticObjectSpriteSet GetSpriteSetByType(MagneticObject.Type type)
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
