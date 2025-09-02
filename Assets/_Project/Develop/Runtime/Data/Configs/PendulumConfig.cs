using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "PendulumConfig", menuName = "Configs/PendulumConfig")]
    public class PendulumConfig : ScriptableObject
    {
        [SerializeField] private float _length = 2f;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _amplitude = 30f;
        [SerializeField] private float _startAngleDeg = 30f;

        public float Length => _length;
        public float Gravity => _gravity;
        public float Amplitude => _amplitude;
        public float StartAngleDeg => _startAngleDeg;
    }
}