using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Presentation.Shared.Views
{
    public class FadeView : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;

        public UniTask FadeTo(float targetAlpha, float duration)
        {
            _fadeImage.DOKill();

            var tcs = new UniTaskCompletionSource();

            _fadeImage
                .DOFade(targetAlpha, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => tcs.TrySetResult());

            return tcs.Task;
        }
    }
}
