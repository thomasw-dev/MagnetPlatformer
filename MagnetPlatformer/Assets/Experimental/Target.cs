using UnityEngine;

namespace Experimental
{
    public class Target : MonoBehaviour
    {
        [Range(0f, 5f)]
        public float Gain = 1f;

        [Range(0f, 10f)]
        public float Radius = 5f;

        [Space(10)]

        [SerializeField] bool _neutral = true;
        [SerializeField] bool _positive = false;
        [SerializeField] bool _negative = false;
        [HideInInspector] public Magnet.Charge Charge;

        [Space(10)]

        [SerializeField] SpriteRenderer _spriteRenderer;

        Color _neutralColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color _positiveColor = new Color(0.67f, 0, 0, 1.0f);
        Color _negativeColor = new Color(0, 0, 0.67f, 1.0f);

        void OnValidate()
        {
            if (Charge == Magnet.Charge.Neutral)
            {
                if (_positive && !_negative)
                    Charge = Magnet.Charge.Positive;
                
                if (!_positive && _negative)
                    Charge = Magnet.Charge.Negative;
            }

            else if (Charge == Magnet.Charge.Positive)
            {
                if (_neutral && !_negative)
                    Charge = Magnet.Charge.Neutral;
                
                if (!_neutral && _negative)
                    Charge = Magnet.Charge.Negative;
            }

            else if (Charge == Magnet.Charge.Negative)
            {
                if (_neutral && !_positive)
                    Charge = Magnet.Charge.Neutral;

                if (!_neutral && _positive)
                    Charge = Magnet.Charge.Positive;
            }

            _neutral = Charge == Magnet.Charge.Neutral;
            _positive = Charge == Magnet.Charge.Positive;
            _negative = Charge == Magnet.Charge.Negative;

            UpdateColor(Charge);
        }

        void UpdateColor(Magnet.Charge charge)
        {
            if (charge == Magnet.Charge.Neutral) _spriteRenderer.color = _neutralColor;
            if (charge == Magnet.Charge.Positive) _spriteRenderer.color = _positiveColor;
            if (charge == Magnet.Charge.Negative) _spriteRenderer.color = _negativeColor;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}