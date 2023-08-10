using HoleDriven.Core;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class ProvideAsyncExample : IAsyncExample
    {
        public string Name => "Provide Values Asynchronously";
        public string Description => "Showing what Hole.ProvideAsync is capable of...";

        public async Task ExecuteAsync()
        {
            var library = await Hole.ProvideAsync("we can also get a value asynchronously", Task.FromResult("HoleDriven.NET"));
            AnsiConsole.WriteLine($"You are using {library}");
        }
    }
}
