using DG.Tweening;
using UnityEngine;

namespace Experimental.DOTweenInterrupt
{
    public class DOTweenInterrupt : MonoBehaviour
    {
        Tweener myTween;

        [Range(0, 3f)]
        [SerializeField] float value = 3f;

        void Start()
        {
            // Countdown (3 -> 0) in 3 seconds

            myTween = DOTween.To(x => value = x, 3f, 0, 3f)
                .SetEase(Ease.Linear)
                .SetAutoKill(false)
                .OnStart(() =>
                {

                })
                .OnUpdate(() =>
                {

                })
                .OnComplete(() =>
                {

                });

            myTween.Pause(); // Pause the tween initially
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myTween.Restart();
            }
        }
    }
}