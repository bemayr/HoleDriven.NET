﻿using System;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static void Effect(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Configuration.ReportHoleEncountered(description, location);
            Configuration.ReportEffectHappened(description, location);
        }
    }
}
