using Data.Configs;

namespace Infrastructure.Fade.Models
{
    public class FadeModel
    {
        public float Duration { get; private set; }

        public FadeModel(GameConfig config)
        {
            Duration = config.SceneFadeDuration;
        }
    }
}
