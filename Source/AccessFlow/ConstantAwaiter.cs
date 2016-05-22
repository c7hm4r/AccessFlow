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
    /// Provides in implementation for an <see cref="IAwaiter{T}"/>
    /// whose value is already known
    /// at the time of creation of the <see cref="IAwaiter{T}"/>.
    /// </summary>
    public static class ConstantAwaiter
    {
        /// <summary>
        /// Creates a new <see cref="IAwaiter{T}"/>
        /// always representing <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the awaited <paramref name="value"/>.
        /// </typeparam>
        /// <param name="value">The awaited value.</param>
        /// <returns>
        /// The new <see cref="IAwaiter{T}"/>
        /// always representing <paramref name="value"/>.
        /// </returns>
        public static IAwaiter<T> Create<T>(T value)
        {
            return new constantAwaiter<T>(value);
        }

        #region private
        private class constantAwaiter<T> : IAwaiter<T>
        {
            public bool IsCompleted => true;

            public T GetResult() => value;

            public void OnCompleted(Action continuation)
            {
                /* Should be enough. Source:
             * http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/Task.cs,4782 */
            }

            public constantAwaiter(T value)
            {
                this.value = value;
            }

            #region private
            private readonly T value;
            #endregion
        }
        #endregion
    }
}