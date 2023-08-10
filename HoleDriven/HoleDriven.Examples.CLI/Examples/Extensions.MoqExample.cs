using HoleDriven.Core;
using HoleDriven.Extension.Moq;
using Moq;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class MoqExample : IExample
    {
        public string Name => "HoleDriven.Extension.Moq";
        public string Description => "Showing what HoleDriven.Extension.Moq is capable of...";

        public void Execute()
        {
            var serviceSetup = Hole.Provide(
                "actually create a Service that does something",
                service => service.Moq<IService>(mock =>
                {
                    mock.Setup(s => s.Save(It.IsAny<string>())).Returns(true);
                }));
            AnsiConsole.MarkupLine("[italic]service1.saved:[/] " + serviceSetup.Save("anything"));

            var serviceLinq = Hole.Provide(
                "actually create a Service that does something",
                service => service.Moq<IService>(mock => mock.Save(It.IsAny<string>()) == true));
            AnsiConsole.MarkupLine("[italic]service2.saved:[/] " + serviceLinq.Save("anything"));
        }
    }

    public interface IService
    {
        public bool Save(string message);
    }
}
