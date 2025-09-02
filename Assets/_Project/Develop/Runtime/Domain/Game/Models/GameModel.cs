using Core.Enums;
using Data.Configs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using R3;


namespace Domain.Game.Models
{
    public class GameModel
    {
        private readonly Dictionary<BallType, int> _matchPoints;
        private readonly int _fullGridChances;

        public Observable<Unit> OnGameOver => _onGameOver;

        private Subject<Unit> _onGameOver = new();

        private int _gridFullCounter;

        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }

        public GameModel(GameConfig gameConfig, BallsConfig config)
        {
            _fullGridChances = gameConfig.FullGridChances;

            _matchPoints = config.Balls
                .ToDictionary(key => key.BallType, value => value.MatchPoints);
        }

        public void Reset()
        {
            _gridFullCounter = 0;
            Score = 0;
            IsGameOver = false;
        }

        public void AddScore(BallType type)
        {
            if (IsGameOver) return;
            Score += _matchPoints[type];
        }

        public void FillGrid()
        {
            _gridFullCounter++;
            if (_gridFullCounter == _fullGridChances) SetGameOver();
        }

        public void SetGameOver()
        {
            IsGameOver = true;
            _onGameOver.OnNext(Unit.Default);
        }

        public BallType GetRandomBall()
        {
            return (BallType)Random.Range(1, 4);
        }
    }
}