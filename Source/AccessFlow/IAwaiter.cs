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
    /// The awaiter of a future.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value represented by the future.
    /// </typeparam>
    /// <remarks>
    /// The structure of this type is required by the .Net framework.
    /// </remarks>
    public interface IAwaiter<out T> : INotifyCompletion
    {
        /// <summary>
        /// Returns if the value of the awaitable is already available and
        /// the execution may proceed synchronously.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Waits for the awaitable to complete and returns
        /// the value represented by it.
        /// </summary>
        /// <returns>The value represented by the awaitable.</returns>
        T GetResult();
    }
}