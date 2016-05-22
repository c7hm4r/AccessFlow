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

namespace AccessFlow.BasicScopes
{
    /// <summary>
    /// An access scope representing an access scope
    /// on each of an optionally open interval of elements.
    /// </summary>
    /// <typeparam name="TSubScope">
    /// The type of the access scope of the elements.
    /// </typeparam>
    public interface IInfiniteIntervalScope<out TSubScope>
    {
        /// <summary>
        /// The inclusive lower index of the interval.
        /// If <c>null</c>,
        /// then the interval does not have a lower bound.
        /// </summary>
        long? LowerBound { get; }

        /// <summary>
        /// The index of the exclusive upper bound of the interval.
        /// If <c>null</c>,
        /// then the interval does not have an upper bound.
        /// </summary>
        long? ProperUpperBound { get; }

        /// <summary>
        /// The access scope on each element within the interval.
        /// </summary>
        TSubScope SubScope { get; }
    }
}