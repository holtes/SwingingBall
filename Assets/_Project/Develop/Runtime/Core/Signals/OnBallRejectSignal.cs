using Core.Enums;

namespace Core.Signals
{
    public class OnBallRejectSignal
    {
        public BallType RejectedBallType;

        public OnBallRejectSignal(BallType ballType) => RejectedBallType = ballType;
    }
}