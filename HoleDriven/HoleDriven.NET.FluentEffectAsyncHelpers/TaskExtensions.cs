using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoleDriven.EffectHelpers
{
    public static class TaskExtensions
    {
        public static Task WithProbabilities(this Task task, double success, double cancelled, double errored)
        {
            if (success + cancelled + errored != 1)
                throw new ArgumentException("fractions should sum upto 1.0");

            var random = new Random().NextDouble();

            if (success > random)
                return task.ContinueWith(_ => Task.CompletedTask);
            else if (success + cancelled > random)
                return task.ContinueWith(_ => Task.FromCanceled(new CancellationToken(true)));
            else
                return task.ContinueWith(_ => Task.FromException(new Exception("this task errored")));
        }
    }
}
