using HoleDriven.Core;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class EffectExample : IExample
    {
        public string Name => "Effects";
        public string Description => "Showing what Hole.Effect is for...";

        public void Execute()
        {
            Hole.Effect("we can also signal that an effect should happen");
        }
    }
}
