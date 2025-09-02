using Infrastructure.Scene;
using Domain.Game.Models;
using Presentation.Shared.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;

namespace Domain.ResultMenu.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private EditableTextView _scoreField;
        [SerializeField] private InteractBtnView _replayBtn;
        [SerializeField] private InteractBtnView _menuBtn;

        private GameModel _gameModel;
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(GameModel gameModel, SceneLoader sceneLoader)
        {
            _gameModel = gameModel;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _replayBtn
                .OnBtnClicked
                .Subscribe(_ => ReplayGame())
                .AddTo(this);

            _menuBtn
                .OnBtnClicked
                .Subscribe(_ => GoToMenu())
                .AddTo(this);

            InitScore();
        }

        private void InitScore()
        {
            _scoreField.SetText(_gameModel.Score);
        }

        private void ReplayGame()
        {
            _sceneLoader.SwitchToScene("Game", "ResultMenu", true).Forget();
        }

        private void GoToMenu()
        {
            _sceneLoader.SwitchToScene("Menu", "ResultMenu", true).Forget();
        }
    }
}