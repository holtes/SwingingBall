using UnityEngine;

namespace Presentation.Game.Views
{
    public class ColumnView : MonoBehaviour
    {
        [SerializeField] private Transform _slotsRoot;

        public void SnapToSlot(Transform ball)
        {
            ball.SetParent(_slotsRoot);
        }
    }
}