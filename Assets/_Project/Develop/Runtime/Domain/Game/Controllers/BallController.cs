using Core.Enums;
using Domain.Game.Models;
using Presentation.Game.Views;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Domain.Game.Controllers
{
    public class BallController : MonoBehaviour, IBallController
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private BallView _view;

        private CancellationTokenSource _cts;

        private BallModel _model;

        [Inject]
        private void Construct(BallModel ballModel)
        {
            _model = ballModel;
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void Init(BallType ballType, Color color)
        {
            _model.SetType(ballType);
            _view.SetBallColor(color);
        }

        public void EnablePhysics()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        public void DisablePhysics()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        public async UniTask CheckRest(CancellationToken token = default)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var ct = _cts.Token;

            var restTime = _model.RestTimeRequired;
            var maxWait = _model.MaxWaitTime;

            var waitTask = UniTask.WaitUntil(() => _model.IsLowVelocity(_rb.linearVelocity.sqrMagnitude), PlayerLoopTiming.Update, ct);
            var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(maxWait), cancellationToken: ct);

            await UniTask.WhenAny(waitTask, timeoutTask);

            float elapsed = 0f;
            while (elapsed < restTime)
            {
                ct.ThrowIfCancellationRequested();

                if (_model.IsLowVelocity(_rb.linearVelocity.sqrMagnitude))
                    elapsed += Time.deltaTime;
                else
                    elapsed = 0f;

                await UniTask.Yield(ct);
            }
        }

        public async UniTask DestroyBall()
        {
            await _view.PlayDestroyEffect();
        }

        public async UniTask DestroyBallWithVFX()
        {
            await _view.PlayDestroyEffectVFX();
        }

        public BallType GetBallType() => _model.BallType;
        public Transform GetTransform() => transform;
    }
}