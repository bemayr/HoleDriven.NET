using Holedriven.Extension.Devtool;
using HoleDriven;
using Microsoft.Extensions.Logging;
using System.Text;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogDebug("Example log message");

// enable Emoji support in Console
Console.OutputEncoding = Encoding.UTF8;

// === Holedriven configuration ===
HoleDriven.Configure.Extensions.ActivateDevtool(frontendUri: "http://localhost:5173");
// ================================

Console.Write("Please tell me who you are: ");
var test = Console.ReadLine();
Console.WriteLine(test);

var person = Hole.Provide(
    "Prompt the User for his name and age",
    value => value.Prompt<Person>());

Console.WriteLine($"Hello {person?.Name}, you are {person?.Age} years old");

record Person(string Name, int Age);
