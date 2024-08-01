using UnityEngine;

[CreateAssetMenu(fileName = "Magnet Sprite Set", order = Constants.MAGNETIC_OBJECT_SPRITE_SET)]
public class MagneticObjectSpriteSet : ScriptableObject
{
    public Sprite Neutral;
    public Sprite Positive;
    public Sprite Negative;

    public Sprite GetSpriteByCharge(Magnet.Charge charge)
    {
        return charge switch
        {
            Magnet.Charge.Neutral => Neutral,
            Magnet.Charge.Positive => Positive,
            Magnet.Charge.Negative => Negative,
            _ => Neutral
        };
    }
}
