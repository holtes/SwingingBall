using Presentation.Shared.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Infrastructure.Fade.Models;

namespace Infrastructure.Fade.Controllers
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private FadeView _view;

        private FadeModel _model;

        [Inject]
        private void Construct(FadeModel fadeModel)
        {
            _model = fadeModel;
        }

        public async UniTask FadeIn()
        {
            await _view.FadeTo(1f, _model.Duration);
        }

        public async UniTask FadeOut()
        {
            await _view.FadeTo(0f, _model.Duration);
        }
    }
}

