using Core.Enums;
using Core.Signals;
using Domain.Game.Models;
using Presentation.Game.Triggers;
using Presentation.Game.Views;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;


namespace Domain.Game.Controllers
{
    public class ColumnController : MonoBehaviour
    {
        [SerializeField] private ColumnView _view;
        [SerializeField] private ColumnSlotTrigger _trigger;

        public Observable<(int, BallType)> OnBallPlaced => _onBallPlaced;
        public Observable<int> OnBallChangedSlot => _onBallChangedSlot;

        private Subject<(int, BallType)> _onBallPlaced = new();
        private Subject<int> _onBallChangedSlot = new();
        private Dictionary<int, IBallController> _balls = new();

        private ColumnModel _model;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(ColumnModel columnModel, SignalBus signalBus)
        {
            _model = columnModel;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _trigger
                .OnBallEntered
                .Subscribe(ball => HandleBallEnter(ball).Forget())
                .AddTo(this);
        }

        private async UniTaskVoid HandleBallEnter(IBallController ball)
        {
            await ball.CheckRest();

            TryPlaceBall(ball);
        }

        private void TryPlaceBall(IBallController ball, bool playerPlaced = true)
        {
            int row = _model.FindFirstEmpty();
            if (row == -1 || !_model.Occupy(row, ball.GetBallType()))
            {
                RejectBall(ball);
                return;
            }

            PlaceBall(row, ball, playerPlaced);
        }

        private void PlaceBall(int row, IBallController ball, bool playerPlaced = true)
        {
            ball.DisablePhysics();
            _balls[row] = ball;

            _view.SnapToSlot(ball.GetTransform());

            _onBallPlaced.OnNext((row, ball.GetBallType()));

            if (playerPlaced) _signalBus.Fire(new OnBallPlacedSignal());
        }

        private void RejectBall(IBallController ball)
        {
            ball.DestroyBall().Forget();
            _signalBus.Fire(new OnBallRejectSignal(ball.GetBallType()));
        }

        public async UniTaskVoid RemoveSlots(int[] removedRows)
        {
            var destroyTasks = new List<UniTask>();

            foreach (var row in removedRows)
            {
                if (!_balls.TryGetValue(row, out var ball)) continue;

                _balls.Remove(row);
                _model.ClearSlot(row);

                destroyTasks.Add(ball.DestroyBallWithVFX());
            }

            await UniTask.WhenAll(destroyTasks);

            DropUpperBalls(removedRows.Min()).Forget();
        }

        private async UniTask DropUpperBalls(int removedRow)
        {
            var upperBallsRows = _balls
                .Where(pair => pair.Key > removedRow)
                .OrderBy(pair => pair.Key)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var row in upperBallsRows)
            {
                _onBallChangedSlot.OnNext(row);

                _model.ClearSlot(row);

                var ball = _balls[row];
                _balls.Remove(row);

                ball.EnablePhysics();

                await ball.CheckRest();

                TryPlaceBall(ball, playerPlaced: false);
            }
        }

        public void ClearColumn()
        {
            _model.ClearColumn();

            foreach (var ball in _balls.Values)
                ball.DestroyBall().Forget();

            _balls.Clear();
        }
    }
}