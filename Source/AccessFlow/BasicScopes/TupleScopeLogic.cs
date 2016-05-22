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
    /// Provides an implementation for an
    /// <see cref="ITupleScopeLogic{T0,T1}"/>.
    /// </summary>
    public static class TupleScopeLogic
    {
        /// <summary>
        /// Creates an <see cref="ITupleScopeLogic{T0,T1}"/>.
        /// </summary>
        /// <typeparam name="T0">The type of the first scope part.</typeparam>
        /// <typeparam name="T1">The type of the second scope part.</typeparam>
        /// <param name="firstLogic">
        /// The logic for the first scope part.
        /// </param>
        /// <param name="secondLogic">
        /// The logic for the second scope part.
        /// </param>
        /// <returns>A new <see cref="ITupleScopeLogic{T0,T1}"/>.</returns>
        public static ITupleScopeLogic<T0, T1> Create<T0, T1>(
            IAccessScopeLogic<T0> firstLogic,
            IAccessScopeLogic<T1> secondLogic)
        {
            return new tupleScopeLogic<T0, T1>(firstLogic, secondLogic);
        }

        #region private
        private class tupleScopeLogic<T0, T1> : ITupleScopeLogic<T0, T1>
        {
            public bool Contains(
                ITupleScope<T0, T1> outer, ITupleScope<T0, T1> inner)
            {
                return firstLogic.Contains(outer.Part0, inner.Part0) &&
                    secondLogic.Contains(outer.Part1, inner.Part1);
            }

            public bool Influences(
                ITupleScope<T0, T1> first, ITupleScope<T0, T1> second)
            {
                return firstLogic.Influences(first.Part0, second.Part0) ||
                    secondLogic.Influences(first.Part1, second.Part1);
            }

            public ITupleScope<T0, T1> Intersect(
                ITupleScope<T0, T1> first, ITupleScope<T0, T1> second)
            {
                return TupleScope.Create(
                    firstLogic.Intersect(first.Part0, second.Part0),
                    secondLogic.Intersect(first.Part1, second.Part1));
            }

            public tupleScopeLogic(
                IAccessScopeLogic<T0> firstLogic,
                IAccessScopeLogic<T1> secondLogic)
            {
                this.firstLogic = firstLogic;
                this.secondLogic = secondLogic;
            }

            #region private
            private readonly IAccessScopeLogic<T0> firstLogic;
            private readonly IAccessScopeLogic<T1> secondLogic;
            #endregion
        }
        #endregion
    }
}