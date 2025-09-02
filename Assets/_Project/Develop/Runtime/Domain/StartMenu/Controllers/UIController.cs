using Infrastructure.Scene;
using Presentation.Shared.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;

namespace Domain.StartMenu.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InteractBtnView _playBtn;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _playBtn
                .OnBtnClicked
                .Subscribe(_ => StartGame())
                .AddTo(this);
        }

        private void StartGame()
        {
            _sceneLoader.SwitchToScene("Game", "Menu", true).Forget();
        }
    }
}