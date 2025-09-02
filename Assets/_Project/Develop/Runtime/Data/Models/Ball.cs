using Core.Enums;
using System;
using UnityEngine;

namespace Data.Models
{
    [Serializable]
    public class Ball
    {
        public Color BallColor = Color.white;
        public BallType BallType = BallType.None;
        public int MatchPoints = 1;
    }
}