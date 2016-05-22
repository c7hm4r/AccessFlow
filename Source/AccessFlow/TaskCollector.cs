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
    /// Provides a basic thread safe implementation for
    /// <see cref="ITaskCollector"/>.
    /// </summary>
    public static class TaskCollector
    {
        /// <summary>
        /// Creates a <see cref="ITaskCollector"/>.
        /// </summary>
        /// <returns>A new <see cref="ITaskCollector"/>.</returns>
        public static ITaskCollector Create()
        {
            return new taskCollector();
        }

        /// <summary>
        /// Executes an asynchronous method <paramref name="content"/>
        /// with a newly created <see cref="ITaskCollector"/>.
        /// All <see cref="Task"/>-s added to the <see cref="ITaskCollector"/>
        /// are awaited.
        /// The asynchronous result of <paramref name="content"/> is
        /// passed as result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="content">
        /// The method executed with the new <see cref="ITaskCollector"/>.
        /// </param>
        /// <returns>
        /// The asynchronous result of <paramref name="content"/>.
        /// </returns>
        public static async Task<T> With<T>(
            Func<ITaskCollector, Task<T>> content)
        {
            ITaskCollector tc = new taskCollector();
            T result = await content(tc);
            await tc.WhenAll();
            return result;
        }

        /// <summary>
        /// Executes an asynchronous method <paramref name="content"/>
        /// with a newly created <see cref="ITaskCollector"/>.
        /// All <see cref="Task"/>-s added to the <see cref="ITaskCollector"/>
        /// are awaited.
        /// </summary>
        /// <param name="content">
        /// The method executed with the new <see cref="ITaskCollector"/>.
        /// </param>
        /// <returns>
        /// A representation of the asynchronous execution.
        /// </returns>
        public static async Task With(
            Func<ITaskCollector, Task> content)
        {
            ITaskCollector tc = new taskCollector();
            await content(tc);
            await tc.WhenAll();
        }

        /// <summary>
        /// Executes a asynchronous method <paramref name="content"/>
        /// with a newly created <see cref="ITaskCollector"/>.
        /// All <see cref="Task"/>-s added to the <see cref="ITaskCollector"/>
        /// are awaited.
        /// The result of <paramref name="content"/> is
        /// passed as result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="content">
        /// The method executed with the new <see cref="ITaskCollector"/>.
        /// </param>
        /// <returns>
        /// The result of <paramref name="content"/> wrapped
        /// in a <see cref="Task"/>.
        /// </returns>
        public static async Task<T> With<T>(
            Func<ITaskCollector, T> content)
        {
            ITaskCollector tc = new taskCollector();
            T result = content(tc);
            await tc.WhenAll();
            return result;
        }

        /// <summary>
        /// Executes a asynchronous method <paramref name="content"/>
        /// with a newly created <see cref="ITaskCollector"/>.
        /// All <see cref="Task"/>-s added to the <see cref="ITaskCollector"/>
        /// are awaited.
        /// </summary>
        /// <param name="content">
        /// The method executed with the new <see cref="ITaskCollector"/>.
        /// </param>
        /// <returns>
        /// A representation of the asynchronous execution.
        /// </returns>
        public static async Task With(
            Action<ITaskCollector> content)
        {
            ITaskCollector tc = new taskCollector();
            content(tc);
            await tc.WhenAll();
        }

        #region private
        private class taskCollector : ITaskCollector
        {
            public void Add(Task task)
            {
                if (task == null)
                    throw new ArgumentNullException(nameof(task));
                tasks.Add(task);
            }

            public IFuture<T> Adding<T>(IReactive<T> reactive)
            {
                Add(reactive.ProcessTask);
                return reactive.Result;
            }

            public Task WhenAll() => Task.WhenAll(tasks.ToArray());

            #region private
            private readonly HashSet<Task> tasks = new HashSet<Task>();
            #endregion
        }
        #endregion
    }
}
