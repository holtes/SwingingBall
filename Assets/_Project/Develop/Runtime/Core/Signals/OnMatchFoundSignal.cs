using Core.Enums;

namespace Core.Signals
{
    public class OnMatchFoundSignal
    {
        public BallType BallType;

        public OnMatchFoundSignal(BallType type) => BallType = type;
    }
}