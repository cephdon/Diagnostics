using System;
#if DNX451
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class ElmScopeNew
    {
        private readonly string _name;
        private readonly object _state;

        public ElmScopeNew(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public ActivityContextNew Context { get; set; }

        public ElmScopeNew Parent { get; set; }

        public ScopeNodeNew Node { get; set; }

#if DNX451
        private static string FieldKey = typeof(ElmScopeNew).FullName + ".Value";
        public static ElmScopeNew Current
        {
            get
            {
                var handle = CallContext.LogicalGetData(FieldKey) as ObjectHandle;

                if (handle == null)
                {
                    return default(ElmScopeNew);
                }

                return (ElmScopeNew)handle.Unwrap();
            }
            set
            {
                CallContext.LogicalSetData(FieldKey, new ObjectHandle(value));
            }
        }
#else
        private static AsyncLocal<ElmScopeNew> _value = new AsyncLocal<ElmScopeNew>();
        public static ElmScopeNew Current
        {
            set
            {
                _value.Value = value;
            }
            get
            {
                return _value.Value;
            }
        }
#endif

        public static IDisposable Push([NotNull] ElmScopeNew scope, [NotNull] ElmStoreNew store)
        {
            var temp = Current;
            Current = scope;
            Current.Parent = temp;

            Current.Node = new ScopeNodeNew()
            {
                //StartTime = DateTimeOffset.UtcNow,
                //State = Current._state,
                //Name = Current._name
            };

            if (Current.Parent != null)
            {
                Current.Node.Parent = Current.Parent.Node;
                Current.Parent.Node.Children.Add(Current.Node);
            }
            else
            {
                Current.Context.Root = Current.Node;
                //store.AddActivity(Current.Context);
            }

            return new DisposableAction(() =>
            {
                Current.Node.EndTime = DateTimeOffset.UtcNow;
                Current = Current.Parent;
            });
        }

        private class DisposableAction : IDisposable
        {
            private Action _action;

            public DisposableAction(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                if (_action != null)
                {
                    _action.Invoke();
                    _action = null;
                }
            }
        }
    }
}
