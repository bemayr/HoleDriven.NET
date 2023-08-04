using HoleDriven.Core;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    [Hole.Idea("we can also use Hole.Idea as an attribute, thus it does not get printed")]
    internal class IdeaExample : IExample
    {
        public string Name => "Noting Ideas";
        public string Description => "Showing what Hole.Idea is for...";

        public void Execute()
        {
            Hole.Idea("Or if we just have an idea on how to improve something, we can jot it down and it applies to the nearest scope");
        }
    }
}
