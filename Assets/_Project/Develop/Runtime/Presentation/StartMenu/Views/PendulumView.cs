using UnityEngine;
using DG.Tweening;

namespace Presentation.StartMenu.Views
{
    public class PendulumView : MonoBehaviour
    {
        [SerializeField] private Transform pendulumPivot;
        [SerializeField] private float swingAngle = 45f;
        [SerializeField] private float duration = 1f;

        private void Start() => AnimatePendulum();

        private void AnimatePendulum()
        {
            pendulumPivot.localRotation = Quaternion.Euler(0, 0, -swingAngle);

            pendulumPivot.DORotate(new Vector3(0, 0, swingAngle), duration)
                         .SetEase(Ease.InOutSine)
                         .SetLoops(-1, LoopType.Yoyo);
        }
    }
}