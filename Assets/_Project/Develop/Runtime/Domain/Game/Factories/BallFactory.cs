using Core.Enums;
using Data.Configs;
using Data.Models;
using Domain.Game.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Domain.Game.Factories
{
    public class BallFactory : IFactory<BallType, Vector3, Transform, BallController>
    {
        private BallController _prefab;
        private DiContainer _container;
        private Dictionary<BallType, Ball> _balls;

        public BallFactory(DiContainer container, BallController ballPrefab, BallsConfig config)
        {
            _prefab = ballPrefab;
            _container = container;
            _balls = config.Balls.ToDictionary(key => key.BallType, value => value);
        }

        public BallController Create(BallType type, Vector3 position, Transform parent)
        {
            var ballGO = _container.InstantiatePrefab(_prefab, position, Quaternion.identity, parent);
            var ball = ballGO.GetComponent<BallController>();

            ball.Init(_balls[type].BallType, _balls[type].BallColor);

            return ball;
        }
    }
}