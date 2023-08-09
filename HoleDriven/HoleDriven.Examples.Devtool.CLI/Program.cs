using Holedriven.Extension.Devtool;
using HoleDriven;
using HoleDriven.Core;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Notepad;
using Serilog.Sinks.Udp.TextFormatters;
using System.Net.Sockets;
using System.Text;

var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.Udp("localhost", 9999, AddressFamily.InterNetwork, new Log4jTextFormatter())
    .WriteTo.Console()
    .CreateLogger();

var loggerFactory = new LoggerFactory()
    .AddSerilog(logger);

loggerFactory.CreateLogger("Some Category").LogError("does this work now?");

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

Console.Read();

record Person(string Name, int Age);
