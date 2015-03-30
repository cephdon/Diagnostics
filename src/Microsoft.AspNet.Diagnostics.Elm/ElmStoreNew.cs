// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class ElmStoreNew
    {
        private const int Capacity = 200;

        private LinkedList<ActivityContextNew> Activities { get; set; } = new LinkedList<ActivityContextNew>();

        /// <summary>
        /// Returns an IEnumerable of the contexts of the logs.
        /// </summary>
        /// <returns>An IEnumerable of <see cref="ActivityContext"/> objects where each context stores 
        /// information about a top level scope.</returns>
        public IEnumerable<ActivityContextNew> GetActivities()
        {
            for (var context = Activities.First; context != null; context = context.Next)
            {
                if (!context.Value.IsCollapsed && CollapseActivityContext(context.Value))
                {
                    Activities.Remove(context);
                }
            }
            return Activities;
        }

        /// <summary>
        /// Adds a new <see cref="ActivityContext"/> to the store.
        /// </summary>
        /// <param name="context">The context to be added to the store.</param>
        public void AddActivity([NotNull] ActivityContextNew activity)
        {
            lock (Activities)
            {
                Activities.AddLast(activity);
                while (Count() > Capacity)
                {
                    Activities.RemoveFirst();
                }
            }
        }

        /// <summary>
        /// Removes all activity contexts that have been stored.
        /// </summary>
        public void Clear()
        {
            Activities.Clear();
        }

        /// <summary>
        /// Returns the total number of logs in all activities in the store
        /// </summary>
        /// <returns>The total log count</returns>
        public int Count()
        {
            return Activities.Sum(a => Count(a.Root));
        }

        private int Count(LogNode node)
        {
            if (node == null)
            {
                return 0;
            }
            
            var scopeNode = node as ScopeNodeNew;
            if(scopeNode == null)
            {
                return 1;
            }

            var sum = 0;
            foreach (var childNode in scopeNode.Children)
            {
                sum += Count(childNode);
            }
            return sum;
        }

        /// <summary>
        /// Removes any nodes on the context's scope tree that doesn't have any logs
        /// This may occur as a result of the filters turned on
        /// </summary>
        /// <param name="context">The context who's node should be condensed</param>
        /// <returns>true if the node has been condensed to null, false otherwise</returns>
        private bool CollapseActivityContext(ActivityContextNew context)
        {
            context.Root = CollapseHelper(context.Root);
            context.IsCollapsed = true;
            return context.Root == null;
        }

        private LogNode CollapseHelper(LogNode node)
        {
            var scopeNode = node as ScopeNodeNew;
            if (node == null)
            {
                return node;
            }

            for (int i = 0; i < scopeNode.Children.Count; i++)
            {
                scopeNode.Children[i] = CollapseHelper(scopeNode.Children[i]);
            }

            scopeNode.Children.RemoveAll(c => c == null);

            //if (node.Children.Count == 0 && node.Messages.Count == 0)
            if (scopeNode.Children.Count == 0)
            {
                return null;
            }
            else
            {
                return node;
            }
        }
    }
}