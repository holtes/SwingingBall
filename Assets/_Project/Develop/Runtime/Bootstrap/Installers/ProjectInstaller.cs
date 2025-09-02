using Data.Configs;
using Domain.Game.Models;
using Infrastructure.Fade.Controllers;
using Infrastructure.Fade.Models;
using Infrastructure.Scene;
using UnityEngine;
using Zenject;

namespace Bootstrap.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private BallsConfig _ballsConfig;

        [Header("Prefabs")]
        [SerializeField] private FadeController _sceneFade;

        public override void InstallBindings()
        {
            BindFadeModel();
            BindSceneFade();
            BindSceneLoader();
            BindGameModel();
        }

        private void BindFadeModel()
        {
            Container
                .Bind<FadeModel>()
                .AsSingle()
                .WithArguments(_gameConfig);
        }

        private void BindSceneFade()
        {
            Container
                .Bind<FadeController>()
                .FromComponentInNewPrefab(_sceneFade)
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<SceneLoader>()
                .AsSingle();
        }

        private void BindGameModel()
        {
            Container
                .Bind<GameModel>()
                .AsSingle()
                .WithArguments(_gameConfig, _ballsConfig);
        }
    }
}