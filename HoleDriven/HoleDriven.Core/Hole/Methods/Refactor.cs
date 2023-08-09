﻿using HoleDriven.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static void Refactor(
            string description,
            Expression<Action> expression, // TODO: maybe capture the expression via Caller...
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Report.HoleEncountered(description, location);
            expression.Compile()();
        }

        public static T Refactor<T>(
            string description,
            Expression<Func<T>> expression,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Report.HoleEncountered(description, location);
            Logger.LogDebug(
                HoleLogEvents.HoleEncountered,
                "Hole encountered {HoleId} {HoleType} {HoleDescription} {HoleLocation}",
                Hole.Provide("Id has to be generated based on the location, think of a generic way that is also compatible with Codegen", "TODO: ID"),
                nameof(Refactor),
                description,
                location);
            return expression.Compile()();
        }

        [Hole.Idea("enable marking of Blocks/Scopes, e.g. NextLine, following if/switch/loop, also make sure to check the correct usage of those scopes with an analyzer")]
        public static void Refactor<T>(
            string description,
            object scope,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Hole.Idea("use the scope in some way or mark it as mandatory");
            Report.HoleEncountered(description, location);
        }
    }
}
