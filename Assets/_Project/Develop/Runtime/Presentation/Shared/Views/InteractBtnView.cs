using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Presentation.Shared.Views
{
    public class InteractBtnView : MonoBehaviour
    {
        [SerializeField] private Button _btnInteract;

        public Observable<Unit> OnBtnClicked => _onBtnClicked;

        private Subject<Unit> _onBtnClicked = new();

        private void Awake()
        {
            _btnInteract
                .OnClickAsObservable()
                .Subscribe(_ => BtnClick())
                .AddTo(this);
        }

        private void BtnClick()
        {
            _onBtnClicked.OnNext(Unit.Default);
        }
    }
}