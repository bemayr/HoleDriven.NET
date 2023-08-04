using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HoleDriven
{
    // using the Singleton pattern for extensibility here (C# does not allow static extension methods)
    public class Configure
    {
        private Configure() { } // hide the public constructor

        private static readonly Lazy<Core.Reporters> _reporters = new Lazy<Core.Reporters>(() => new Core.Reporters()); // TODO: "maybe create something like Reporters.Default
        private static readonly Core.IExtensionMarker _extensions = new Core.ExtensionMarker();

        public static Core.Reporters Reporters => _reporters.Value;
        public static Core.IExtensionMarker Extensions => _extensions;
    }
}
