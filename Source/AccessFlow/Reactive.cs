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
using System.Linq;
using System.Threading.Tasks;

namespace AccessFlow
{
    /// <summary>
    /// Provides an implementation for an <see cref="IReactive{T}"/>
    /// based on its public components.
    /// </summary>
    public static class Reactive
    {
        /// <summary>
        /// Creates an <see cref="IReactive{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The result of the function execution represented
        /// by the created <see cref="IReactive{T}"/>.
        /// </typeparam>
        /// <param name="processTask">
        /// The representation of the complete execution of the execution.
        /// </param>
        /// <param name="result">
        /// The promise of the result of the function execution.
        /// </param>
        /// <returns>A new <see cref="IReactive{T}"/>.</returns>
        public static IReactive<T> Create<T>(
            Task processTask, IFuture<T> result)
        {
            return new reactive<T>(processTask, result);
        }

        /// <summary>
        /// Runs the computation <paramref name="content"/> with
        /// a new <see cref="IResolver{T}"/> to resolve its result and
        /// returns a new <see cref="IReactive{T}"/> representing
        /// the computation.
        /// </summary>
        /// <typeparam name="T">The type of the computation result.</typeparam>
        /// <param name="content">The computation to execute.</param>
        /// <returns>
        /// A new <see cref="IReactive{T}"/> representing the computation.
        /// </returns>
        public static IReactive<T> From<T>(
            this Func<IResolver<T>, Task> content)
        {
            var resolverAndFuture = Resolver.CreateWithFuture<T>();
            Task processTask = content(resolverAndFuture.Item1);
            return new reactive<T>(processTask, resolverAndFuture.Item2);
        }

        #region private
        private class reactive<T> : IReactive<T>
        {
            public Task ProcessTask { get; }
            public IFuture<T> Result { get; }

            public reactive(Task processTask, IFuture<T> result)
            {
                ProcessTask = processTask;
                Result = result;
            }
        }
        #endregion
    }
}
