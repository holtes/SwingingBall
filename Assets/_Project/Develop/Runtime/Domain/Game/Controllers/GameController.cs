using Core.Signals;
using Infrastructure.Scene;
using Domain.Game.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;

namespace Domain.Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private GameModel _model;
        private SceneLoader _sceneLoader;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(GameModel gameModel, SceneLoader sceneLoader, SignalBus signalBus)
        {
            _model = gameModel;
            _sceneLoader = sceneLoader;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnBallPlacedSignal>(BallPlaced);
            _signalBus.Subscribe<OnMatchFoundSignal>(MatchFound);
            _signalBus.Subscribe<OnBallRejectSignal>(BallRejected);
            _signalBus.Subscribe<OnGridFilledSignal>(GridFilled);

            _model
                .OnGameOver
                .Subscribe(_ => EndGame())
                .AddTo(this);

            _model.Reset();
        }

        private void Start()
        {
            _signalBus.Fire(new OnSpawnNextBallSignal(_model.GetRandomBall()));
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnBallPlacedSignal>(BallPlaced);
            _signalBus.Unsubscribe<OnMatchFoundSignal>(MatchFound);
            _signalBus.Unsubscribe<OnBallRejectSignal>(BallRejected);
            _signalBus.Unsubscribe<OnGridFilledSignal>(GridFilled);
        }

        private void BallRejected(OnBallRejectSignal signal)
        {
            _signalBus.Fire(new OnSpawnNextBallSignal(signal.RejectedBallType));
        }

        private void BallPlaced()
        {
            if (_model.IsGameOver) EndGame();
            else _signalBus.Fire(new OnSpawnNextBallSignal(_model.GetRandomBall()));
        }

        private void MatchFound(OnMatchFoundSignal signal)
        {
            _model.AddScore(signal.BallType);
        }

        private void GridFilled(OnGridFilledSignal signal)
        {
            _model.FillGrid();
        }

        private void EndGame()
        {
            _sceneLoader.SwitchToScene("ResultMenu", "Game", additive: false).Forget();
        }
    }
}