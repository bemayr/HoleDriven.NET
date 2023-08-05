namespace HoleDriven
{
    [Hole.Idea("maybe Effect and Provide can be collapsed into Mock")]
    [Hole.Idea("maybe we can get rid of the async variations then (they would be automatically discoverd and async exection could be defined via a parameter)")]
    [Hole.Idea("provide the Mock attribute for partial methods, which would allow mock code generation (maybe with AutoBogus and C# 11's generic attributes)")]
    [Hole.Idea("integrate AutoBogus")]
    [Hole.Idea("switch configuration to a standard approach like https://github.com/nickdodd79/AutoBogus#conventions")]
    [Hole.Idea("enable **Markdown** in holes, e.g. using [Markdig](https://github.com/xoofx/markdig) for stripping out markdown while reporting the holes using the analyzer")]
    public static partial class Hole
    {
        public static Core.Reporters Report => Configure.Reporters;
    }
}
