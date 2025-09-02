using Core.Enums;
using Data.Configs;

namespace Domain.Game.Models
{
    public class BallModel
    {
        public float RestVelocityThreshold { get; private set; }
        public float RestTimeRequired { get; private set; }
        public float MaxWaitTime { get; private set; }

        public BallType BallType { get; private set; }

        public BallModel(BallsConfig config)
        {
            RestVelocityThreshold = config.RestVelocityThreshold;
            RestTimeRequired = config.RestTimeRequired;
            MaxWaitTime = config.MaxWaitTime;
        }

        public void SetType(BallType ballType)
        {
            BallType = ballType;
        }

        public bool IsLowVelocity(float ballVelocity)
        {
            return ballVelocity <= RestVelocityThreshold * RestTimeRequired;
        }
    }
}