using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[CreateAssetMenu(fileName = "Magnet Sprite Set", order = 2)]
public class MagnetSpriteSet : ScriptableObject
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
