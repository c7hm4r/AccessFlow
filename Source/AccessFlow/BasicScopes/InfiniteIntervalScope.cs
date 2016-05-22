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
    /// Provides an implementation for
    /// <see cref="IInfiniteIntervalScope{TSubScope}"/>.
    /// </summary>
    public static class InfiniteIntervalScope
    {
        /// <summary>
        /// Creates an <see cref="IInfiniteIntervalScope{TSubScope}"/>.
        /// </summary>
        /// <typeparam name="TSubScope">
        /// The type of the access scope of the interval elements.
        /// </typeparam>
        /// <param name="subScope">
        /// The access scope on each element within the interval.
        /// </param>
        /// <param name="lowerBound">
        /// The inclusive lower index of the interval.
        /// If <c>null</c>,
        /// then the interval does not have a lower bound.
        /// </param>
        /// <param name="properUpperBound">
        /// The index of the exclusive upper bound of the interval.
        /// If <c>null</c>,
        /// then the interval does not have an upper bound.
        /// </param>
        /// <returns>
        /// A new <see cref="IInfiniteIntervalScope{TSubScope}"/>.
        /// </returns>
        public static IInfiniteIntervalScope<TSubScope> Create<TSubScope>(
            TSubScope subScope, long? lowerBound = 0,
            long? properUpperBound = 0)
        {
            return new infiniteIntervalScope<TSubScope>(
                subScope, lowerBound, properUpperBound);
        }

        #region private
        private class infiniteIntervalScope<TSubScope> :
            IInfiniteIntervalScope<TSubScope>
        {
            public long? LowerBound { get; }
            public long? ProperUpperBound { get; }
            public TSubScope SubScope { get; }

            public override string ToString()
            {
                return $"Start: {LowerBound}; " +
                       $"ProperUpperBound: {ProperUpperBound}; " +
                       $"SubScope: ({SubScope})";
            }

            public infiniteIntervalScope(TSubScope subScope,
                long? lowerBound = 0, long? properUpperBound = 0)
            {
                if (lowerBound != null && properUpperBound != null &&
                    lowerBound.Value > properUpperBound.Value)
                {
                    throw new ArgumentException(
                        "MUST NOT but was: LowerBound != null && " +
                        "echterUpperBound != null && " +
                        "LowerBound.Value < echterUpperBound.Value");
                }

                LowerBound = lowerBound;
                ProperUpperBound = properUpperBound;
                SubScope = subScope;
            }
        }
        #endregion
    }
}
