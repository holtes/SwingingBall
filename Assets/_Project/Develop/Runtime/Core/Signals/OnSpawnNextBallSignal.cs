using Core.Enums;

namespace Core.Signals
{
    public class OnSpawnNextBallSignal
    {
        public BallType BallType;

        public OnSpawnNextBallSignal(BallType ballType) => BallType = ballType;
    }
}