using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using R3;

namespace Presentation.Game.Views
{
    public class BallView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ballRenderer;
        [SerializeField] private ParticleSystem _magicExplosionFX;

        public void SetBallColor(Color color)
        {
            _ballRenderer.color = color;
        }

        public async UniTask PlayDestroyEffect()
        {
            var tcs = new UniTaskCompletionSource();

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.3f))
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                    tcs.TrySetResult();
                });

            await tcs.Task;
        }

        public async UniTask PlayDestroyEffectVFX()
        {
            var tcs = new UniTaskCompletionSource();

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.3f))
                .OnComplete(() =>
                {
                    PlayExplosionVFX();
                    Destroy(gameObject);
                    tcs.TrySetResult();
                });

            await tcs.Task;
        }

        private void PlayExplosionVFX()
        {
            Instantiate(_magicExplosionFX, transform.position,
                transform.rotation, transform.parent);
        }
    }
}