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
    /// Collects <see cref="Task"/>-s and
    /// can bunch them to a new Task representing
    /// the finish of all collected <see cref="Task"/>-s.
    /// </summary>
    public interface ITaskCollector
    {
        /// <summary>
        /// Collects a <see cref="Task"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to collect.</param>
        void Add(Task task);

        /// <summary>
        /// Collects the <see cref="IReactive{T}.ProcessTask"/> of an
        /// <see cref="IReactive{T}"/> and returns its 
        /// <see cref="IReactive{T}.Result"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value represented by
        /// the <see cref="IReactive{T}"/>.
        /// </typeparam>
        /// <param name="reactive">
        /// The <see cref="IReactive{T}"/> whose
        /// <see cref="IReactive{T}.ProcessTask"/> shall be collected.
        /// </param>
        /// <returns>
        /// The <see cref="IReactive{T}.Result"/> of
        /// <paramref name="reactive"/>.
        /// </returns>
        IFuture<T> Adding<T>(IReactive<T> reactive);

        /// <summary>
        /// Returns a <see cref="Task"/>
        /// representing the finish of all collected <see cref="Task"/>-s.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/>
        /// representing the finish of all collected <see cref="Task"/>-s.
        /// </returns>
        Task WhenAll();
    }
}