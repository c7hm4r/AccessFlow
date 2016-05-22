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
    /// Provides an implementation for <see cref="IResolver{T}"/> based on
    /// <see cref="TaskCompletionSource{TResult}"/>.
    /// </summary>
    public static class Resolver
    {
        /// <summary>
        /// Creates a pair of a new <see cref="IResolver{T}"/> and
        /// a new <see cref="TaskCompletionSource{TResult}"/> backing the
        /// <see cref="IResolver{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to be resolved.
        /// </typeparam>
        /// <returns>
        /// A pair of a new <see cref="IResolver{T}"/> and
        /// a new <see cref="TaskCompletionSource{TResult}"/> backing the
        /// <see cref="IResolver{T}"/>.
        /// </returns>
        public static Tuple<IResolver<T>, TaskCompletionSource<T>>
            CreateWithTcs<T>()
        {
            var tcs = TaskCompletionSourceHelper.CreateAsyncTcs<T>();
            IResolver<T> r = new resolver<T>(tcs);
            return Tuple.Create(r, tcs);
        }

        /// <summary>
        /// Creates a new <see cref="IResolver{T}"/> based on the given
        /// <paramref name="tcs"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to be resolved.
        /// </typeparam>
        /// <param name="tcs">
        /// The <see cref="TaskCompletionSource{TResult}"/> used as backend
        /// for the created <see cref="IResolver{T}"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="IResolver{T}"/> based on the given
        /// <paramref name="tcs"/>.
        /// </returns>
        public static IResolver<T> Create<T>(TaskCompletionSource<T> tcs)
        {
            return new resolver<T>(tcs);
        }

        /// <summary>
        /// Creates a pair of a new <see cref="IResolver{T}"/> and
        /// new <see cref="IFuture{T}"/> linked together.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to be passed from
        /// the <see cref="IResolver{T}"/> to the <see cref="IFuture{T}"/>.
        /// </typeparam>
        /// <returns>
        /// A pair of a new <see cref="IResolver{T}"/> and
        /// new <see cref="IFuture{T}"/> linked together.
        /// </returns>
        public static Tuple<IResolver<T>, IFuture<T>> CreateWithFuture<T>()
        {
            TaskCompletionSource<T> tcs =
                TaskCompletionSourceHelper.CreateAsyncTcs<T>();
            IResolver<T> resolver = Create(tcs);
            IFuture<T> result = TaskFuture.Create(tcs);
            return Tuple.Create(resolver, result);
        }

        #region private
        private class resolver<T> : IResolver<T>
        {
            public void Resolve(T value)
            {
                tcs.SetResult(value);
            }

            public resolver(TaskCompletionSource<T> tcs)
            {
                this.tcs = tcs;
            }

            #region private
            private readonly TaskCompletionSource<T> tcs;
            #endregion
        }
        #endregion
    }
}
