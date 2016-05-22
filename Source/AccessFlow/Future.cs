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
    /// Provides an implementation for an <see cref="IFuture{T}"/>
    /// based on an <see cref="IAwaiter{T}"/>.
    /// </summary>
    public static class Future
    {
        /// <summary>
        /// Creates an <see cref="IFuture{T}"/>
        /// based on an <see cref="IAwaiter{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented by the <see cref="IFuture{T}"/>.
        /// </typeparam>
        /// <param name="awaiter">
        /// The <see cref="IAwaiter{T}"/> the
        /// <see cref="IFuture{T}"/> to create is based on.
        /// </param>
        /// <returns>The newly created <see cref="IFuture{T}"/>.</returns>
        public static IFuture<T> Create<T>(IAwaiter<T> awaiter)
        {
            return new future<T>(awaiter);
        }

        #region private
        private class future<T> : IFuture<T>
        {
            public T Value => awaiter.GetResult();

            public IAwaiter<T> GetAwaiter() => awaiter;

            public future(IAwaiter<T> awaiter)
            {
                this.awaiter = awaiter;
            }

            #region private
            private readonly IAwaiter<T> awaiter;
            #endregion
        }
        #endregion
    }
}
