using System;
using System.Threading.Tasks;

namespace HoleDriven.EffectHelpers
{
    public static class EffectAsyncExtensions
    {
        public static WithDuration ThatTakesAround(this Hole.IEffectAsyncInput _, TimeSpan duration)
        {
            return new WithDuration(duration);
        }
    }

    public class WithDuration : Core.IEffectAsyncResult
    {
        public WithDuration(TimeSpan duration)
        {
            Duration = duration;
        }
        public Task Task => Task.Delay(Duration);

        public TimeSpan Duration { get; }
    }
}
