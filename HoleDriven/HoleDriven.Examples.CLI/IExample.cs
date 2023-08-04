namespace HoleDriven.Examples.CLI
{
    internal interface IExampleBase
    {
        public string Name { get; }
        public string Description { get; }
    }

    internal interface IExample : IExampleBase
    {
        void Execute();
    }

    internal interface IAsyncExample : IExampleBase
    {
        Task ExecuteAsync();
    }
}