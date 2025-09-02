using UnityEngine;
using R3;
using R3.Triggers;


namespace Presentation.Game.Triggers
{
    public class ColumnSlotTrigger : MonoBehaviour
    {
        public Observable<IBallController> OnBallEntered => _onBallEntered;

        private Subject<IBallController> _onBallEntered = new();

        private void Awake()
        {
            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(collider => DetectEnterances(collider))
                .AddTo(this);
        }

        private void DetectEnterances(Collider2D other)
        {
            if (other.TryGetComponent(out IBallController ball))
                _onBallEntered.OnNext(ball);
        }
    }
}