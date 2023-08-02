using Holedriven;
using System.Linq.Expressions;

//ExampleConventional();
ExampleHoleDriven();

static void ExampleConventional()
{
    Console.WriteLine("What is your Name?");
    var name = Console.ReadLine();
    Console.WriteLine($"Hello, {name}!");
}

static void ExampleHoleDriven()
{
    Console.WriteLine("What is your Name?");
    var name = Hole.Get("Read the name from the command line", "Bernhard");
    Hole.Set("Write a greeting that greets the user");
    //Hole.Refactor(() => Console.WriteLine($"Hello, {name}!"));

    Hole.Throw("some bogus");
}

//static void HoleAPI()
//{
//    Hole.Get();
//    Hole.Refactor();
//    Hole.Set();
//    Hole.SetAsync();
//    Hole.Throw();
//    Hole.Idea();
//}

Expression<Func<int>> myExpression = () => 10;
Expression<Action> log = () => Console.WriteLine("test");