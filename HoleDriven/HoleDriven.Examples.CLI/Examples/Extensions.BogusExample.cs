using HoleDriven.Core;
using HoleDriven.Extension.Bogus;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class BogusExample : IExample
    {
        public string Name => "HoleDriven.Extension.Bogus";
        public string Description => "Showing what HoleDriven.Extension.Bogus is capable of...";

        public void Execute()
        {
            var user = Hole.Provide(
                "get some random [red]user[/]",
                value => value.Bogus<User>(f => f.RuleFor(o => o.FirstName, f => f.Name.FirstName())));

            var name = Hole.Provide(
                "ask the user to enter his/her real username",
                value => value.Bogus(f => f.Name.FirstName()));
        }

        private record User
        {
            public string FirstName { get; init; } = default!;
        }
    }
}
