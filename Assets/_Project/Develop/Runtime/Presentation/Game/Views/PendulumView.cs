using UnityEngine;

namespace Presentation.Game.Views
{
    public class PendulumView : MonoBehaviour
    {
        [SerializeField] private FixedJoint2D _ballJoint;
        [SerializeField] private Rigidbody2D _ballJointRb;
        [SerializeField] private Transform _pivot;
        [SerializeField] private LineRenderer _line;

        private float _ropeLength;

        public void Init(float ropeLength)
        {
            _ropeLength = ropeLength;
        }

        public void AttachBall(Rigidbody2D ballRb)
        {
            _ballJoint.connectedBody = ballRb;
        }

        public void DeattachBall()
        {
            _ballJoint.connectedBody = null;
            ResetLine();
        }

        private void ResetLine()
        {
            _line.SetPosition(0, _pivot.position);
            _line.SetPosition(1, _pivot.position);
        }

        public void UpdatePendulum(float angleDeg)
        {
            var rad = angleDeg * Mathf.Deg2Rad;
            var targetLocal = new Vector3(Mathf.Sin(rad), -Mathf.Cos(rad), 0) * _ropeLength;
            var targetWorld = _pivot.position + targetLocal;

            _ballJointRb.MovePosition(targetWorld);

            _line.SetPosition(0, _pivot.position);
            _line.SetPosition(1, _ballJointRb.position);
        }
    }
}