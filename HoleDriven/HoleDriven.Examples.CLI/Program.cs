﻿using HoleDriven;
using System.Text;
using Spectre.Console;

// enable Emoji support in Console
Console.OutputEncoding = Encoding.UTF8;

// Customize Reporters
//HoleDriven.Configuration.RemoveDefaultHoleEncounteredReporter();
//HoleDriven.Configuration.RemoveDefaultEffectHappenedReporter();
//HoleDriven.Configuration.EffectHappenedReporter += (description, location) =>
//{
//    var fileName = Path.GetFileName(location.FilePath);
//    var formattedLocation = $"{location.CallerMemberName} in {fileName}:line {location.LineNumber}";
//    AnsiConsole.MarkupLine($"🧩 [bold invert turquoise2][[EFFECT 🥏]][/]: [italic]{description}[/] [dim](at {formattedLocation})[/]");
//};

HoleDriven.Extension.PrettyConsoleReporters.Activate();

// This is an example of an application that uses all possible Holes

var simpleValue = Hole.Provide("we can simply provide a value and the type is inferred automatically", 42);
var asyncValue = await Hole.ProvideAsync("we can also get a value asynchronously", Task.FromResult("HoleDriven.NET"));
Hole.Effect("we can also signal that an effect should happen");
await Hole.EffectAsync("this effect can also happen asynchronously");
try { Hole.Throw("if we want to throw an exception upon encountering a Hole, we can simply use `Hole.Throw`"); } catch { }
var pi = Hole.Refactor(
    "we can also signal that something should be refactored, e.g. this literal should be replaced with a Math.PI",
    () => 3.1415);
Hole.Refactor(
    "we can also mark some statement in a way that it needs refactoring, e.g. that the following secure PIN should be printed as ****",
    () => AnsiConsole.WriteLine("Pin Code: 0000"));
Hole.Idea("Or if we just have an idea on how to improve something, we can jot it down and it applies to the nearest scope");

namespace AttributeExample
{
    [Hole.Idea("think of whether to include a direction in which we do not want to steer")]
    public enum Direction { Left, Down, Up }
}