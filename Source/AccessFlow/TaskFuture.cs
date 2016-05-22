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
    /// Provides conversions from <see cref="Task"/> or
    /// <see cref="TaskCompletionSource{TResult}"/> to
    /// <see cref="IFuture{T}"/>.
    /// </summary>
    public static class TaskFuture
    {
        /// <summary>
        /// Wraps the <see cref="Task"/> <paramref name="core"/> in
        /// a new <see cref="IFuture{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented by
        /// the result <see cref="IFuture{T}"/> and <paramref name="core"/>.
        /// </typeparam>
        /// <param name="core">The <see cref="Task"/> to be wrapped.</param>
        /// <returns>
        /// A new <see cref="IFuture{T}"/> wrapping
        /// the <see cref="Task"/> <paramref name="core"/>.
        /// </returns>
        public static IFuture<T> ToFuture<T>(this Task<T> core)
        {
            return Create(core);
        }

        /// <summary>
        /// Wraps the <see cref="Task"/> <paramref name="task"/> in
        /// a new <see cref="IFuture{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented by
        /// the result <see cref="IFuture{T}"/> and <paramref name="task"/>.
        /// </typeparam>
        /// <param name="task">The <see cref="Task"/> to be wrapped.</param>
        /// <returns>
        /// A new <see cref="IFuture{T}"/> wrapping
        /// the <see cref="Task"/> <paramref name="task"/>.
        /// </returns>
        public static IFuture<T> Create<T>(Task<T> task)
        {
            return Future.Create(AwaiterForTask.Create(task.GetAwaiter()));
        }

        /// <summary>
        /// Wraps the <see cref="TaskCompletionSource{TResult}.Task"/> of
        /// <paramref name="tcs"/> a new <see cref="IFuture{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented by
        /// the result <see cref="IFuture{T}"/> and the
        /// <see cref="TaskCompletionSource{TResult}.Task"/> of
        /// <paramref name="tcs"/>.
        /// </typeparam>
        /// <param name="tcs">
        /// The <see cref="TaskCompletionSource{TResult}"/> whose
        /// <see cref="TaskCompletionSource{TResult}.Task"/> shall be wrapped.
        /// </param>
        /// <returns>
        /// A new <see cref="IFuture{T}"/> wrapping
        /// the <see cref="TaskCompletionSource{TResult}.Task"/> of
        /// <paramref name="tcs"/>.
        /// </returns>
        public static IFuture<T> Create<T>(TaskCompletionSource<T> tcs)
        {
            return Create(tcs.Task);
        }
    }
}
