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
    /// Provides an implementation for an <see cref="ITupleScope{T0,T1}"/>.
    /// </summary>
    public static class TupleScope
    {
        /// <summary>Creates a <see cref="ITupleScope{T0,T1}"/>.</summary>
        /// <typeparam name="T0">The type of the first scope part.</typeparam>
        /// <typeparam name="T1">The type of the second scope part.</typeparam>
        /// <param name="part0">The first scope part.</param>
        /// <param name="part1">The second scope part.</param>
        /// <returns>A new <see cref="ITupleScope{T0,T1}"/>.</returns>
        public static ITupleScope<T0, T1> Create<T0, T1>(T0 part0, T1 part1)
        {
            return new tupleScope<T0, T1>(part0, part1);
        }

        #region private
        private class tupleScope<T0, T1> : ITupleScope<T0, T1>
        {
            public T0 Part0 { get; }
            public T1 Part1 { get; }

            public tupleScope(T0 part0, T1 part1)
            {
                Part0 = part0;
                Part1 = part1;
            }
        }
        #endregion
    }
}
