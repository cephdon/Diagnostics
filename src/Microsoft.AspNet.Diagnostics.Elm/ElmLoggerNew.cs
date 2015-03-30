// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Framework.Logging;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class ElmLoggerNew : ILogger
    {
        private readonly string _name;
        private readonly ElmOptions _options;
        private readonly ElmStoreNew _store;

        public ElmLoggerNew(string name, ElmOptions options, ElmStoreNew store)
        {
            _name = name;
            _options = options;
            _store = store;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, 
                          Func<object, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || (state == null && exception == null))
            {
                return;
            }

            var messageNode = new MessageNode()
            {
                ActivityContext = GetCurrentActivityContext(),
                Name = _name,
                EventID = eventId,
                Severity = logLevel,
                Exception = exception,
                State = state,
                Message = formatter == null ? state.ToString() : formatter(state, exception),
                Time = DateTimeOffset.UtcNow
            };

            if (ElmScopeNew.Current != null)
            {
                ElmScopeNew.Current.Node.Children.Add(messageNode);
            }
            // The log does not belong to any scope - create a new context for it
            else
            {
                var context = GetNewActivityContext();
                //context.RepresentsScope = false;  // mark as a non-scope log
                context.Root = messageNode;
                _store.AddActivity(context);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _options.Filter(_name, logLevel);
        }

        public IDisposable BeginScope(object state)
        {
            var scope = new ElmScopeNew(_name, state);
            scope.Context = ElmScopeNew.Current?.Context ?? GetNewActivityContext();
            return ElmScopeNew.Push(scope, _store);
        }

        private ActivityContextNew GetNewActivityContext()
        {
            return new ActivityContextNew()
            {
                Id = Guid.NewGuid(),
                Time = DateTimeOffset.UtcNow,
                //RepresentsScope = true
            };
        }

        private ActivityContextNew GetCurrentActivityContext()
        {
            return ElmScopeNew.Current?.Context ?? GetNewActivityContext();
        }
    }
}