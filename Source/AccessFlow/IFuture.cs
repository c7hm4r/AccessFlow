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
    /// Provides a similar interface as <see cref="Task{TResult}"/>,
    /// but supports a covariant generic parameter.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value encapsulated by the Future.
    /// </typeparam>
    /// <remarks>
    /// Objects implementing this interface can be awaited.
    /// </remarks>
    public interface IFuture<out T>
    {
        /// <summary>
        /// Waits for the <see cref="IFuture{T}"/> to complete and returns
        /// the value represented by it.
        /// </summary>
        T Value { get; }

        // ReSharper disable once UnusedMethodReturnValue.Global
        /// <summary>
        /// Returns the awaiter for the async/await language feature.
        /// </summary>
        /// <returns>The awaiter for the async/await language feature</returns>
        /// <remarks>
        /// The method is required by the .Net Framework.
        /// </remarks>
        IAwaiter<T> GetAwaiter();
    }
}
