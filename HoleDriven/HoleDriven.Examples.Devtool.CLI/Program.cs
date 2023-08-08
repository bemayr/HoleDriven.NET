using Holedriven.Extension.Devtool;
using HoleDriven;

// === Holedriven configuration ===
HoleDriven.Configure.Extensions.ActivateDevtool();
// ================================

Console.Write("Please tell me who you are: ");

var person = Hole.Provide(
    "Prompt the User for his name and age",
    value => value.Prompt<Person>());

Console.WriteLine($"Hello { person?.Name }, you are { person?.Age } years old");

record Person(string Name, int Age);