using HoleDriven;
using System.Text;
using Spectre.Console;
using HoleDriven.Examples.CLI.Examples;
using HoleDriven.Examples.CLI;

// enable Emoji support in Console
Console.OutputEncoding = Encoding.UTF8;

var examples = new IExampleBase[]
{
    new RefactorExample(),
    new IdeaExample(),
    new EffectExample(),
    new EffectAsyncExample(),
    new ProvideExample(),
    new ProvideAsyncExample(),
    new ThrowExample(),
    new CustomizeReportersExample(),
    new PrettyConsoleReportersExample(),
    new TaskHelpersExample(),
    new BogusExample(),
    new MoqExample()
};

AnsiConsole.Write(new FigletText("HoleDriven.NET").LeftJustified().Color(Color.Green));
foreach (var example in examples)
    await Run(example);

async Task Run(IExampleBase example)
{
    AnsiConsole.Write(new Rule($"[invert] {example.Name} [/]").Centered());
    AnsiConsole.MarkupLine($"{example.Description}");
    AnsiConsole.WriteLine();
    switch (example)
    {
        case IExample syncExample:
            syncExample.Execute(); break;
        case IAsyncExample asyncExample:
            await asyncExample.ExecuteAsync(); break;
    }
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine();
}

namespace HoleDriven.Examples.CLI
{
    public static class StringExtensions
    {
        public static string PadBoth(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}