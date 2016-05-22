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
    /// The return value of an asynchronous operation, which produces a result.
    /// The result can be available before the operation has completed.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public interface IReactive<out T>
    {
        /// <summary>
        /// The execution of the operation.
        /// Needs to be awaited so that no exceptions are ignored.
        /// </summary>
        Task ProcessTask { get; }

        /// <summary>
        /// The productive result of the operation.
        /// </summary>
        IFuture<T> Result { get; }
    }
}