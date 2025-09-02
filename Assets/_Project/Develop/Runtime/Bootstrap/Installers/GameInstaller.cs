using Core.Signals;
using Data.Configs;
using Domain.Game.Controllers;
using Domain.Game.Factories;
using Domain.Game.Input;
using Domain.Game.Models;
using UnityEngine;
using Zenject;

namespace Bootstrap.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private PendulumConfig _pendulumConfig;
        [SerializeField] private BallsConfig _ballsConfig;

        [Header("Scene")]
        [SerializeField] private PlayerInputHandler _playerInput;

        [Header("Prefabs")]
        [SerializeField] private BallController _ballPrefab;

        public override void InstallBindings()
        {
            BindSignalBus();
            BindSignals();
            BindBallFactory();
            BindPlayerInput();
            BindPendulumModel();
            BindColumnModel();
            BindCollectorModel();
            BindBallModel();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);
        }

        private void BindSignals()
        {
            Container.DeclareSignal<OnBallPlacedSignal>();
            Container.DeclareSignal<OnMatchFoundSignal>();
            Container.DeclareSignal<OnSpawnNextBallSignal>();
            Container.DeclareSignal<OnBallRejectSignal>();
            Container.DeclareSignal<OnGridFilledSignal>();
        }

        private void BindBallFactory()
        {
            Container
            .Bind<BallFactory>()
            .AsSingle()
            .WithArguments(_ballPrefab, _ballsConfig);
        }

        private void BindPlayerInput()
        {
            Container
                .Bind<PlayerInputHandler>()
                .FromInstance(_playerInput);
        }

        private void BindPendulumModel()
        {
            Container
                .Bind<PendulumModel>()
                .AsSingle()
                .WithArguments(_pendulumConfig);
        }

        private void BindColumnModel()
        {
            Container
                .Bind<ColumnModel>()
                .AsTransient()
                .WithArguments(_gameConfig);
        }

        private void BindCollectorModel()
        {
            Container
                .Bind<CollectorModel>()
                .AsSingle()
                .WithArguments(_gameConfig);
        }

        private void BindBallModel()
        {
            Container
                .Bind<BallModel>()
                .AsTransient()
                .WithArguments(_ballsConfig);
        }
    }
}