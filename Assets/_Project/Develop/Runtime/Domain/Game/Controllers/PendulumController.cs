using Core.Signals;
using Domain.Game.Factories;
using Domain.Game.Input;
using Domain.Game.Models;
using Presentation.Game.Views;
using UnityEngine;
using Zenject;
using R3;


namespace Domain.Game.Controllers
{
    public class PendulumController : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private PendulumView _view;

        private BallController _attachedBall;
        private CompositeDisposable _swingingStream = new CompositeDisposable();

        private PendulumModel _model;
        private PlayerInputHandler _input;
        private BallFactory _ballFactory;
        private SignalBus _signalBus;

        [Inject]
        private void Construct
        (
            PendulumModel pendulumModel,
            PlayerInputHandler playerInput,
            BallFactory ballFactory,
            SignalBus signalBus
        )
        {
            _model = pendulumModel;
            _input = playerInput;
            _ballFactory = ballFactory;
            _signalBus = signalBus;
        }

        private void Init()
        {
            _view.Init(_model.GetLength());
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnSpawnNextBallSignal>(SpawnBall);

            _input
                .OnCutRopeInput
                .Subscribe(_ => ReleaseBall())
                .AddTo(this);

            Init();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnSpawnNextBallSignal>(SpawnBall);

            _swingingStream.Dispose();
        }

        private void UpdatePendulumAngle()
        {
            var angle = _model.GetAngle(Time.deltaTime);
            _view.UpdatePendulum(angle);
        }

        private void SpawnBall(OnSpawnNextBallSignal signal)
        {
            _attachedBall = _ballFactory.Create(signal.BallType, _spawnPoint.position, _spawnPoint);
            var ballRb = _attachedBall.GetComponent<Rigidbody2D>();

            _attachedBall.DisablePhysics();

            _view.AttachBall(ballRb);
            _model.AttachBall(ballRb.mass, ballRb.angularDamping);

            Observable
                .EveryUpdate()
                .Subscribe(_ => UpdatePendulumAngle())
                .AddTo(_swingingStream);
        }

        private void ReleaseBall()
        {
            if (!_attachedBall) return;

            _swingingStream.Clear();

            _attachedBall.EnablePhysics();
            _attachedBall.transform.SetParent(transform.parent);
            _attachedBall = null;

            _view.DeattachBall();
        }
    }
}