using Data.Configs;
using UnityEngine;

namespace Domain.Game.Models
{
    public class PendulumModel
    {
        private readonly float _pendulumLength;
        private readonly float _pendulumGravity;

        private float _angle;
        private float _angularVel;

        public float AttachedBallMass { get; private set; }
        public float AttachedBallAngularDamping { get; private set; }

        public PendulumModel(PendulumConfig config)
        {
            _pendulumLength = config.Length;
            _pendulumGravity = config.Gravity;

            _angle = config.StartAngleDeg * Mathf.Deg2Rad;
            _angularVel = 0f;
        }

        public float GetAngle(float deltaTime)
        {
            float angularAcc = -(_pendulumGravity / _pendulumLength) *
                Mathf.Sin(_angle) - AttachedBallAngularDamping * _angularVel / AttachedBallMass;

            _angularVel += angularAcc * deltaTime;
            _angle += _angularVel * deltaTime;

            return _angle * Mathf.Rad2Deg;
        }

        public float GetLength()
        {
            return _pendulumLength;
        }

        public void AttachBall(float mass, float angularDamping)
        {
            AttachedBallMass = mass;
            AttachedBallAngularDamping = angularDamping;
        }
    }
}