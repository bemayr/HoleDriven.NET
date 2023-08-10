using Holedriven.Extension.Devtool;
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


#region Fakes
var value = Hole.Fake("value", 42);
var asyncValue = await Hole.Fake("desc", Task.Delay(1000).ContinueWith(_ => 42));
var lazyValue = Hole.Fake("lazy value", () => Environment.GetEnvironmentVariable("WORLD"));
var asyncLazyValue = await Hole.Fake("async value", async () =>
{
    await Task.Delay(1000);
    return 42;
});
var extensionValue = Hole.Fake("prompt person via extension", value => value.Prompt<Person>());
var asyncExtensionValue = await Hole.Fake("choose person asynchronously via extension", value => value.ChooseAsync(new[] {
    new Person("John", 50),
    new Person("Jane", 25)
}));
Hole.Fake("log", () => Console.WriteLine("effect"));
Hole.Fake("log via extension", hole => hole.Log());
await Hole.Fake("save asynchronously", Task.Delay(500));
await Hole.Fake("save asynchronously", () => Task.Delay(500));
await Hole.Fake("async extended effect", effect => effect.Progress(3000));
#endregion


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

static class Extensions
{
    public static TValue Prompt<TValue>(this Hole.IFakeExtension input, string explanation = "")
    {
        Console.WriteLine($"Prompting: {input.Information.Description} ({explanation})");
        return default;
    }

    public async static Task<TValue> ChooseAsync<TValue>(this Hole.IFakeExtension _, IEnumerable<TValue> choices, string explanation = "")
    {
        await Console.Out.WriteLineAsync(explanation);
        await Task.Delay(500);
        return choices.First();
    }

    public static void Log(this Hole.IFakeExtension input)
    {
        Console.WriteLine(input.Information.Description);
    }

    public async static Task Progress(this Hole.IFakeExtension _, int duration)
    {
        await Task.Delay(duration);
    }
}