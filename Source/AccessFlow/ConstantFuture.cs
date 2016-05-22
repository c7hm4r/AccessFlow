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
    /// Provides an implementation for a <see cref="IFuture{T}"/> whose
    /// value is already known at the time of creation.
    /// </summary>
    public static class ConstantFuture
    {
        /// <summary>
        /// Creates an <see cref="IFuture{T}"/> whose
        /// value is already known at the time of creation.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented
        /// by the created <see cref="IFuture{T}"/>.
        /// </typeparam>
        /// <param name="value">
        /// The value represented by the created <see cref="IFuture{T}"/>.
        /// </param>
        /// <returns>The newly created <see cref="IFuture{T}"/>.</returns>
        /// <remarks>
        /// This method can be invoked on an object of any type.
        /// </remarks>
        public static IFuture<T> ToFuture<T>(this T value)
        {
            return Future.Create(ConstantAwaiter.Create(value));
        }
    }
}
