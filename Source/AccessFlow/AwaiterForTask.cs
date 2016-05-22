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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AccessFlow
{
    /// <summary>
    /// Provides an implementation of <see cref="IAwaiter{T}"/>
    /// based on <see cref="TaskAwaiter{TResult}"/>.
    /// </summary>
    public static class AwaiterForTask
    {
        /// <summary>
        /// Creates a new <see cref="IAwaiter{T}"/>
        /// which wraps the given <see cref="TaskAwaiter{TResult}"/>
        /// <paramref name="core"/>.
        /// </summary>
        /// <param name="core">
        /// The <see cref="TaskAwaiter{TResult}"/> to be wrapped.
        /// </param>
        /// <typeparam name="T">
        /// The type of the value which is awaited.
        /// </typeparam>
        /// <returns>
        /// The new new <see cref="IAwaiter{T}"/>
        /// which wraps the given <see cref="TaskAwaiter{TResult}"/>
        /// <paramref name="core"/>.
        /// </returns>
        public static IAwaiter<T> Create<T>(TaskAwaiter<T> core)
        {
            return new awaiterForTask<T>(core);
        }

        #region private
        private class awaiterForTask<T> : IAwaiter<T>
        {
            public bool IsCompleted => core.IsCompleted;

            public T GetResult() => core.GetResult();

            public void OnCompleted(Action continuation) =>
                core.OnCompleted(continuation);

            public awaiterForTask(TaskAwaiter<T> core)
            {
                this.core = core;
            }

            #region private
            private TaskAwaiter<T> core;
            #endregion
        }
        #endregion
    }
}
