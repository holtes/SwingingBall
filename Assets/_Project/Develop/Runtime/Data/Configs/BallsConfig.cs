using Data.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "BallsConfig", menuName = "Configs/BallsConfig")]
    public class BallsConfig : ScriptableObject
    {
        [SerializeField] private float _restVelocityThreshold = 0.07f;
        [SerializeField] private float _restTimeRequired = 0.12f;
        [SerializeField] private float _maxWaitTime = 2f;
        [SerializeField] private List<Ball> _balls;

        public float RestVelocityThreshold => _restVelocityThreshold;
        public float RestTimeRequired => _restTimeRequired;
        public float MaxWaitTime => _maxWaitTime;
        public List<Ball> Balls => _balls;
    }
}