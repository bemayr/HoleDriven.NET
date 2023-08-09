using Holedriven.Extension.Devtool;
using HoleDriven;
using HoleDriven.Core;
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
logger.LogWarning("Example log message");
Console.WriteLine("fadsf");

Holes.Configure(holes => holes
    .SetLogger(loggerFactory)
    .UseDevtool(frontendUri: "http://localhost:5173"));

// enable Emoji support in Console
Console.OutputEncoding = Encoding.UTF8;

Console.Write("Please tell me who you are: ");
Hole.Refactor("remove this", () => Console.WriteLine(Console.ReadLine()));

System.Diagnostics.Trace.WriteLine("this is traced");

var person = Hole.Provide(
    "Prompt the User for his name and age",
    value => value.Prompt<Person>());

Console.WriteLine($"Hello {person?.Name}, you are {person?.Age} years old");

record Person(string Name, int Age);
