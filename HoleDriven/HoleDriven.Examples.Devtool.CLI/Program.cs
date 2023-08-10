using Holedriven.Extension.Devtool;
using HoleDriven;
using HoleDriven.Core;
using HoleDriven.Core.Reporters;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text;

EnableEmojisInConsole();

Holes.Configure(holes => holes
    .SetLogger(CreateLoggerFactory())
    .SetReporters(reporters =>
    {
        reporters.HoleEncountered += (HoleEncountered.Params hole) =>
        {
            if (hole.Type == HoleDriven.Core.Types.HoleType.Fake)
                Console.WriteLine("🧩");
        };
    })
    .UseDevtool(frontendUri: "http://localhost:5173"));

Console.Write("Please tell me who you are: ");
var person = Hole.Provide(
    "Prompt the User for his/her Name and Age (look up how to read from the command line)",
    value => value.Prompt<Person>());

Console.WriteLine($"Hello {person?.Name}, you are {person?.Age} years old");
WaitForKeyPressed();

static void EnableEmojisInConsole()
{
    Console.OutputEncoding = Encoding.UTF8;
}
static ILoggerFactory CreateLoggerFactory()
{
    var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

    var loggerFactory = new LoggerFactory()
        .AddSerilog(logger);

    return loggerFactory;
}
static void WaitForKeyPressed()
{
    Console.WriteLine("Press any key to exit the application...");
    Console.Read();
}

record Person(string Name, int Age);
