using Holedriven.Extension.Devtool;
using HoleDriven;
using HoleDriven.Core;
using Microsoft.Extensions.Logging;
using Serilog;

Hole.Idea("Currently it is not possible to Set the reporters, change this and show an example", Scope.NextLine);
Holes.Configure(holes => holes
    .SetLogger(CreateLoggerFactory())
    .UseDevtool(frontendUri: "http://localhost:5173"));

Console.Write("Please tell me who you are: ");
var person = Hole.Provide(
    "Prompt the User for his name and age",
    value => value.Prompt<Person>());

Console.WriteLine($"Hello {person?.Name}, you are {person?.Age} years old");

Console.WriteLine("Press any key to exit the application...");
Console.Read();

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

record Person(string Name, int Age);
