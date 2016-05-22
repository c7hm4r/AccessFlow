#region copyright
// Copyright 2016 Christoph Müller
// 
// This file is part of AccessFlow.
// 
// AccessFlow is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AccessFlow is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AccessFlow.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LockFreeDoublyLinkedLists;

namespace AccessFlow
{
    /// <summary>
    /// Provides the default implementation for
    /// <see cref="IAccessContext{TScope}"/>.
    /// </summary>
    public static class AccessContext
    {
        /// <summary>
        /// Creates an <see cref="IAccessContext{TScope}"/>.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <param name="accessScopeLogic">
        /// The type of access scope logic.
        /// </param>
        /// <param name="accessScope">
        /// The access scope of the new root access context.
        /// Every access scope
        /// which is derived from the access scope created by this method
        /// will have an access scope contained by
        /// <paramref name="accessScope"/>.
        /// </param>
        /// <returns>
        /// The new created <see cref="IAccessContext{TScope}"/>.
        /// </returns>
        public static IAccessContext<TScope> Create<TScope>(
            IAccessScopeLogic<TScope> accessScopeLogic,
            TScope accessScope)
        {
            return new accessContext<TScope>(accessScopeLogic, accessScope);
        }

        #region private
        private class accessContext<TScope> : IAccessContext<TScope>
        {
            public IAccessScopeLogic<TScope> AccessScopeLogic
            { get; }

            public bool IsActive
            {
                get
                {
                    AccessContextState state = node.Value.State;
                    return state == AccessContextState.Active
                        || state == AccessContextState.AcitiveInitializing;
                }
            }

            public TScope RequiredScope
            {
                get { return node.Value.AccessScope; }
                set { reduceRequiredAccessScope(previousScope => value); }
            }

            public void IntersectRequiredScopeWith(TScope maximumAccessScope)
            {
                reduceRequiredAccessScope(previousScope =>
                    AccessScopeLogic.Intersect(
                    previousScope, maximumAccessScope));
            }

            public async Task UntilAvailable()
            {
                AccessContextInfo<TScope> current = node.Value;
                /* waitForAsync must be awaited,
                 * to retain the chain of the passing of Task.Yield(). */
                await waitForAsync(current.AccessScope, current);
            }

            public async Task UntilAvailable(TScope scopeToOwn)
            {
                AccessContextInfo<TScope> current = node.Value;
                if (!AccessScopeLogic.Contains(
                    current.AccessScope, scopeToOwn))
                {
                    throw newArgumentOutOfScopeException(
                        nameof(scopeToOwn), scopeToOwn, current.AccessScope);
                }
                /* waitForAsync must be awaited,
                 * to retain the chain of the passing of Task.Yield(). */
                await waitForAsync(scopeToOwn, current);
            }

            public IAccessContext<TScope> CreateChild(TScope accessScope)
            {
                initializeAndCorrectNextOverhangingNodeProperty(node);
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                    newNode = node.InsertAfterIf(
                        new AccessContextInfo<TScope>(
                            accessScope,
                            AccessContextState.AcitiveInitializing, null,
                            new MultiEvent<ILockFreeDoublyLinkedListNode<
                                    AccessContextInfo<TScope>>,
                                AccessContextInfo<TScope>>()),
                        new childContextCreationChecker(
                            AccessScopeLogic, accessScope).Check);
                if (newNode == null)
                {
                    if (node.Value.State == AccessContextState.Ended)
                        throw new InvalidOperationException(
                            "The parent context has already been disposed.");
                    else
                    {
                        var exc = new InvalidOperationException(
                            "The requested access scope has not been " +
                            "within the scope of the parent context. \n" +
                            $"parent scope: ({node.Value.AccessScope}); \n" +
                            $"child scope: ({accessScope}).");
                        throw exc;
                    }
                }
                initializeAndCorrectNextOverhangingNodeProperty(newNode);
                return new accessContext<TScope>(
                    AccessScopeLogic, newNode);
            }

            public IAccessContext<TScope> CreateChildWithin()
            {
                return createSubContextWithin(parentScope => parentScope);
            }

            public IAccessContext<TScope> CreateChildWithin(
                TScope maximumAccessScope)
            {
                return createSubContextWithin(parentScope =>
                    AccessScopeLogic.Intersect(
                        maximumAccessScope, parentScope));
            }

            public bool Disposed
            {
                get
                {
                    /* Übernommen aus Dispose(). */
                    AccessContextInfo<TScope> current = node.Value;
                    initializeAndCorrectNextOverhangingNodeProperty(
                        node, ref current);
                    return current.State == AccessContextState.Ended;
                }
            }

            public void Dispose()
            {
                AccessContextInfo<TScope> current = node.Value;
                initializeAndCorrectNextOverhangingNodeProperty(
                    node, ref current);
                while (true)
                {
                    if (current.State == AccessContextState.Ended)
                        return;

                    AccessContextInfo<TScope> newInfo =
                        new AccessContextInfo<TScope>(
                            current.AccessScope, AccessContextState.Ended,
                            current.NextOverhangingNode,
                            current.RequiredAccessScopeReduced);

                    AccessContextInfo<TScope> prevalent
                        = node.CompareExchangeValue(newInfo, current);

                    if (current == prevalent)
                    {
                        current = newInfo;
                        break;
                    }

                    current = prevalent;
                }
                current.RequiredAccessScopeReduced.InvokeAll(node, current);
                node.Remove();
            }

            public accessContext(IAccessScopeLogic<TScope> asLogic,
                TScope accessScope)
            {
                if (asLogic == null)
                    throw new ArgumentNullException();

                var list = LockFreeDoublyLinkedList.Create<
                    AccessContextInfo<TScope>>();
                node = list.PushRight(
                    new AccessContextInfo<TScope>(
                        accessScope, AccessContextState.Active,
                        null, new MultiEvent<ILockFreeDoublyLinkedListNode<
                                AccessContextInfo<TScope>>,
                            AccessContextInfo<TScope>>()));
                AccessScopeLogic = asLogic;
            }

            #region private
            private readonly
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node;

            private void reduceRequiredAccessScope(
                Func<TScope, TScope> newScopeFromParentScope)
            {
                initializeAndCorrectNextOverhangingNodeProperty(node);
                AccessContextInfo<TScope> current = node.Value;
                while (true)
                {
                    if (current.State == AccessContextState.Ended)
                        throw new InvalidOperationException(
                            "The current context has already been disposed.");
                    TScope newAccessScope
                        = newScopeFromParentScope(current.AccessScope);
                    if (!AccessScopeLogic.Contains(
                        current.AccessScope, newAccessScope))
                    {
                        throw newArgumentOutOfScopeException(
                            nameof(newAccessScope), newAccessScope,
                            current.AccessScope);
                    }
                    AccessContextInfo<TScope> newInfo =
                        new AccessContextInfo<TScope>(
                            newAccessScope,
                            AccessContextState.AcitiveInitializing,
                            null, current.RequiredAccessScopeReduced);
                    AccessContextInfo<TScope> prevalent =
                        node.CompareExchangeValue(newInfo, current);
                    if (prevalent == current)
                    {
                        current = newInfo;
                        break;
                    }
                    current = prevalent;
                }

                current.RequiredAccessScopeReduced.InvokeAll(node, current);
            }

            private accessContext<TScope> createSubContextWithin(
                Func<TScope, TScope> newScopeFromParentScope)
            {
                initializeAndCorrectNextOverhangingNodeProperty(node);
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                    newNode;
                while (true)
                {
                    TScope newScope =
                        newScopeFromParentScope(node.Value.AccessScope);
                    newNode = node.InsertAfterIf(
                        new AccessContextInfo<TScope>(
                            newScope,
                            AccessContextState.AcitiveInitializing, null,
                            new MultiEvent<
                                ILockFreeDoublyLinkedListNode<
                                        AccessContextInfo<TScope>>,
                                AccessContextInfo<TScope>>()),
                        new childContextCreationChecker(
                            AccessScopeLogic, newScope).Check);
                    if (newNode == null)
                    {
                        if (node.Value.State == AccessContextState.Ended)
                            throw new InvalidOperationException(
                                "The parent context " +
                                "has already been disposed.");
                    }
                    else
                        break;
                }
                initializeAndCorrectNextOverhangingNodeProperty(newNode);
                return new accessContext<TScope>(
                    AccessScopeLogic, newNode);
            }

            /* Under certain circumstances
             * (e.g. if the scope which shall be waited for
             * is already awailable at the time of the method invocation)
             * a new completed Task is beeing returned.
             * The consequence of that is,
             * that the execution is not interrupted at the position
             * where context.UntilAvailable is being awaited.
             * This behavior is not desired,
             * as it may delay the awaiting method returning its Task.
             * In my opinion the execution should be interrupted by default,
             * as that it seems to be the most consistent behavior.
             * Maybe an optional parameter should be provided
             * which allows the behavior to be deactivated
             * concerning performance.
            /* Ways for implementation:
             * - employ Task.Yield()
             * - implement an own Awaitable. */

            private async Task waitForAsync(
                TScope scopeToOwn, AccessContextInfo<TScope> current)
            {
                /* Incomplete protection against parallel execution. */
                if (!current.IsActive)
                    throw new InvalidOperationException();

                Tuple<ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>,
                    AccessContextInfo<TScope>>
                    findResult = findInfluencingPredecessor(
                    node, current, scopeToOwn, AccessScopeLogic);
                if (findResult == null)
                    return; // no interruption

                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                    influencingNode = findResult.Item1;
                AccessContextInfo<TScope> influencingNodeValue =
                    findResult.Item2;

                TaskCompletionSource<object> waitCompletionSource =
                    TaskCompletionSourceHelper.CreateAsyncTcs<object>();

                var handler = new accessScopeReducedHandler(
                    scopeToOwn, waitCompletionSource, AccessScopeLogic);
                influencingNodeValue.RequiredAccessScopeReduced.Enqueue(
                    handler.Invoke);

                influencingNodeValue = influencingNode.Value;
                if (!AccessScopeLogic.Influences(
                    influencingNodeValue.AccessScope, scopeToOwn))
                {
                    handler.Invoke(influencingNode, influencingNodeValue);
                }
                await waitCompletionSource.Task;
                
                // possibly no interruption
            }

            private accessContext(
                IAccessScopeLogic<TScope> asLogic,
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node)
            {
                AccessScopeLogic = asLogic;
                this.node = node;
            }

            ~accessContext()
            {
                if (node != null && AccessScopeLogic != null)
                    Dispose();
            }

            private static
                Tuple<ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>,
                    AccessContextInfo<TScope>>
                findInfluencingPredecessor(
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node,
                AccessContextInfo<TScope> nodeValue, TScope accessScope,
                IAccessScopeLogic<TScope> accessScopeLogic)
            {
                while (true)
                {
                    node = initializeAndCorrectNextOverhangingNodeProperty
                            (node, ref nodeValue);
                    if (node == null)
                        return null;
                    nodeValue = node.Value;
                    if (accessScopeLogic.Influences(
                        nodeValue.AccessScope, accessScope))
                    {
                        return new Tuple<ILockFreeDoublyLinkedListNode<
                                AccessContextInfo<TScope>>,
                            AccessContextInfo<TScope>>(node, nodeValue);
                    }
                }
            }

            private static ILockFreeDoublyLinkedListNode<
                    AccessContextInfo<TScope>>
                initializeAndCorrectNextOverhangingNodeProperty(
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node,
                ref AccessContextInfo<TScope> current)
            {
                if (current == null)
                    current = node.Value;
                while (true)
                {
                    if (current.State !=
                        AccessContextState.AcitiveInitializing)
                    {
                        break;
                    }
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                        nextOverhangingNode = getNextNotEndedNode(node);
                    AccessContextInfo<TScope> newInfo =
                        new AccessContextInfo<TScope>(
                            current.AccessScope,
                            AccessContextState.Active, nextOverhangingNode,
                            current.RequiredAccessScopeReduced);
                    AccessContextInfo<TScope> prevalent
                        = node.CompareExchangeValue(newInfo, current);
                    if (prevalent == current)
                    {
                        current = newInfo;
                        break;
                    }
                    current = prevalent;
                }
                return correctNextOverhangingNodeProperty(node, ref current);
            }

            // ReSharper disable once UnusedMethodReturnValue.Local
            private static ILockFreeDoublyLinkedListNode<
                    AccessContextInfo<TScope>>
                initializeAndCorrectNextOverhangingNodeProperty(
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node)
            {
                AccessContextInfo<TScope> current = null;
                return initializeAndCorrectNextOverhangingNodeProperty(
                    node, ref current);
            }

            /// <summary>
            /// Handles the case where the stored
            /// next overhangig observedNode has been deleted.
            /// It sets the next overhanging observedNode
            /// to its next not ended observedNode.
            /// </summary>
            /// <param name="node">
            /// The observedNode to correct
            /// the next overhanging observedNode reference for.
            /// </param>
            /// <param name="current">
            /// The <c>Value</c> of <paramref name="node"/>, already read
            /// (for efficiency).
            /// </param>
            /// <returns>
            /// The correct next overhangig observedNode
            /// of <paramref name="node"/>.
            /// </returns>
            private static ILockFreeDoublyLinkedListNode<
                    AccessContextInfo<TScope>>
                correctNextOverhangingNodeProperty(
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node,
                ref AccessContextInfo<TScope> current)
            {
                Debug.Assert(current != null);
                while (true)
                {
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                        currentNextOverhangingNode
                            = current.NextOverhangingNode;
                    if (currentNextOverhangingNode == null
                        || currentNextOverhangingNode.Value.State
                            != AccessContextState.Ended)
                    {
                        return currentNextOverhangingNode;
                    }
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                        newNextOverhangingNode
                            = getNextNotEndedNode(currentNextOverhangingNode);
                    AccessContextInfo<TScope> newInfo =
                        new AccessContextInfo<TScope>(
                            current.AccessScope, current.State,
                            newNextOverhangingNode,
                            current.RequiredAccessScopeReduced);
                    AccessContextInfo<TScope> prevalent
                        = node.CompareExchangeValue(newInfo, current);
                    current = prevalent == current ? newInfo : prevalent;
                }
            }

            private static ILockFreeDoublyLinkedListNode<
                    AccessContextInfo<TScope>>
                getNextNotEndedNode(
                ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>> node)
            {
                do
                {
                    node = node.Next;
                    if (node == node.List.Tail)
                        return null;

                    /* Placeholder contexts are 
                     * valid “next overhanging nodes” (for search), too. */
                } while (node.Value.State == AccessContextState.Ended);
                return node;
            }

            private static ArgumentOutOfRangeException
                newArgumentOutOfScopeException(string paramName,
                    TScope paramValue, TScope parentValue)
            {
                return new ArgumentOutOfRangeException(paramName, paramValue,
                    $"{paramName} must be contained in " +
                    "the current access scope (which is " +
                    $"{parentValue}).");
            }

            private class childContextCreationChecker
            {
                public bool Check(AccessContextInfo<TScope> parentInfo)
                {
                    return parentInfo.State == AccessContextState.Active
                            && asLogic.Contains(
                                parentInfo.AccessScope, newAccessScope);
                }

                public childContextCreationChecker(
                    IAccessScopeLogic<TScope> asLogic,
                    TScope newAccessScope)
                {
                    this.asLogic = asLogic;
                    this.newAccessScope = newAccessScope;
                }

                #region private
                private readonly IAccessScopeLogic<TScope> asLogic;
                private readonly TScope newAccessScope;
                #endregion
            }

            private class accessScopeReducedHandler
                : IMultiEventHandler<
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>,
                    AccessContextInfo<TScope>>
            {
                public void Invoke(
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                        observedNode,
                    AccessContextInfo<TScope> obsNodeValue)
                {
                    oneTimeHandler.Invoke(observedNode, obsNodeValue);
                }

                public accessScopeReducedHandler(
                    TScope accessScope, TaskCompletionSource<object> tcs,
                    IAccessScopeLogic<TScope> asi)
                {
                    this.accessScope = accessScope;
                    taskCompletionSource = tcs;
                    scopeLogic = asi;
                    oneTimeHandler = OneTimeAction<
                            ILockFreeDoublyLinkedListNode<
                                AccessContextInfo<TScope>>,
                            AccessContextInfo<TScope>>
                        .Create(invokeInner);
                    Thread.MemoryBarrier();
                }

                #region private
                private readonly TScope accessScope;

                private readonly TaskCompletionSource<object>
                    taskCompletionSource;

                private readonly IAccessScopeLogic<TScope>
                    scopeLogic;

                private readonly Action<
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>,
                    AccessContextInfo<TScope>> oneTimeHandler;

                private void invokeInner(
                    ILockFreeDoublyLinkedListNode<AccessContextInfo<TScope>>
                        observedNode,
                    AccessContextInfo<TScope> obsNodeValue)
                {
                    Thread.MemoryBarrier();
                    if (!obsNodeValue.IsActive
                        || !scopeLogic.Influences(
                            obsNodeValue.AccessScope, accessScope))
                    {
                        AccessContextInfo<TScope> observedNodeValue =
                            observedNode.Value;
                        Tuple<ILockFreeDoublyLinkedListNode<
                                AccessContextInfo<TScope>>,
                            AccessContextInfo<TScope>> findResult =
                                findInfluencingPredecessor(observedNode,
                                    observedNodeValue, accessScope,
                                    scopeLogic);
                        if (findResult == null)
                        {
                            taskCompletionSource.SetResult(null);
                            return;
                        }
                        observedNode = findResult.Item1;
                        obsNodeValue = findResult.Item2;
                    }
                    else
                    {
                        /* Der aktuelle Knoten beansprucht
                         * noch immer einen Teil der Ressource. */
                    }
                    var handler = new accessScopeReducedHandler(
                        accessScope, taskCompletionSource, scopeLogic);
                    obsNodeValue.RequiredAccessScopeReduced.Enqueue(
                        handler.Invoke);
                    obsNodeValue = observedNode.Value;
                    if (!scopeLogic.Influences(
                        obsNodeValue.AccessScope, accessScope))
                    {
                        /* Unendliche Rekursion möglich. → Speicherproblem? */
                        handler.Invoke(observedNode, obsNodeValue);
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
