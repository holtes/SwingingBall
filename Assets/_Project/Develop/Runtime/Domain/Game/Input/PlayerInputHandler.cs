using UnityEngine;
using R3;

namespace Domain.Game.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Observable<Unit> OnCutRopeInput => _onCutRopeInput;

        private Subject<Unit> _onCutRopeInput = new();

        public void OnCutRope()
        {
            _onCutRopeInput.OnNext(Unit.Default);
        }
    }
}